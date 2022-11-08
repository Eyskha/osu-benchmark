// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// May need to remove constructors that are assignable to Monobehvaiour for Unity projects

namespace osu.Game.ModChloe
{
    public static class ModLogExecutionData
    {
        public static string ProjectName = "";
        public static string PathAssemblyToInspect = "";
        public static string[] NamespacesToInspect = new string[] { };
        public static string ExecutionEnvironment = "";
        public static Type[] TypesToExclude = new Type[] { };
        public static List<(string, string)> MethodsToExclude = new List<(string, string)> { }; // Methods raising a Harmony Exception. Format: (class, method)

        public static void SetModParameters(string projectName, string pathAssembly, string[] namespaces, string environment, Type[] typesToExclude, List<(string, string)> methodsToExclude)
        {
            ProjectName = projectName;
            PathAssemblyToInspect = pathAssembly;
            NamespacesToInspect = namespaces;
            ExecutionEnvironment = environment;
            TypesToExclude = typesToExclude;
            MethodsToExclude = methodsToExclude;
        }

        public static void Patch()
        {
            //Harmony.DEBUG = true;
            FileLog.Reset();

            // Log file path
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ModData", ProjectName);
            string fileName = $"{ProjectName}_{ExecutionEnvironment}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm")}.json";
            Environment.SetEnvironmentVariable("HARMONY_LOG_FILE", Path.Combine(folderPath, fileName));

            // Execution info
            FileLog.Log($"{{\n" +
                            $"\"Assembly\": \"{PathAssemblyToInspect.Replace("\\", "/")}\",\n" +
                            $"\"Namespaces\": {JsonConvert.SerializeObject(NamespacesToInspect)},\n" +
                            $"\"ExecutionEnvironment\": \"{ExecutionEnvironment}\",\n" +
                            $"\"MethodsPatches\": [");

            // Patch
            var harmony = new Harmony("com.test.harmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        // Handle edge cases, TODO: resolve patching problems for those edge case methods
        public static IEnumerable<Type> GetTypesInNamespaceToInspect()
        {
            return AccessTools.GetTypesFromAssembly(Assembly.LoadFrom(PathAssemblyToInspect))
                .Where(type => NamespacesToInspect.Contains(type.Namespace) // TODO
                            && !TypesToExclude.Contains(type)
                            && !type.IsValueType // to remove enum and struct:s from the analysis
                            && !type.IsInterface // no interfaces
                            && !type.IsGenericType // no generic class
                            && !type.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)
                            && !type.FullName.Contains("DisplayClass"));
        }


        public static readonly JsonSerializerSettings JSON_SETTINGS = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        };

        public static void AddObjectToLogFile(object objectToSerialize)
        {
            FileLog.Log(JsonConvert.SerializeObject(objectToSerialize, JSON_SETTINGS));
            FileLog.Log(",");
        }
    }

    //[HarmonyPatch(typeof(EditorBeatmap))]
    //[HarmonyPatch("Add")]
    [HarmonyPatch]
    public class MyPatchesAllMethodsAndConstructors
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return ModLogExecutionData.GetTypesInNamespaceToInspect()
                .SelectMany(type => type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                        .Where(method => type == method.DeclaringType
                                                      && !method.IsAbstract // Otherwise error because no body
                                                      && !method.IsSpecialName // To exclude set and get methods in properties
                                                      && !method.IsGenericMethod // no generic method
                                                      && MethodBaseExtensions.HasMethodBody(method)
                                                      && !ModLogExecutionData.MethodsToExclude.Contains((type.Name, method.Name)))
                                        .Cast<MethodBase>()
                                    .Concat(type.GetConstructors()
                                                .Cast<MethodBase>()));
        }

        // Prefix
        public static void Prefix(MethodBase __originalMethod)
        {
            // Stack trace
            var trace = new StackTrace();
            //patch.stackTrace = trace.GetFrames().Select(frame => frame.ToString()).ToList();
            ModLogExecutionData.AddObjectToLogFile(new MethodPatch
            {
                Fqn = __originalMethod.FullDescription(),
                Timestamp = DateTime.Now,
                StackTraceListFQNs = trace.GetFrames().Select(frame => frame.GetMethod().FullDescription()).ToList(),
                DepthStackTrace = trace.FrameCount
            });
        }
    }

    public class MethodPatch
    {
        public string? Fqn;
        public DateTime? Timestamp;
        public List<string>? StackTraceListFQNs;
        public int? DepthStackTrace;
    }
}
