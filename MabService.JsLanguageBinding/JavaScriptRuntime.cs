using Jint;
using Jint.Native;
using System;

namespace MabService.JsLanguageBinding
{
    /// <summary>
    /// JsEngineFactory
    /// </summary>
    internal class JavaScriptRuntime
    {
        /// <summary>
        /// Compiles the specified API code.
        /// </summary>
        /// <param name="apiCode">The API code.</param>
        public static void Compile(string apiCode)
        {
            CreateEngine().Execute(apiCode);
        }

        /// <summary>
        /// Runs the specified apid code.
        /// </summary>
        /// <param name="apidCode">The apid code.</param>
        /// <param name="requestContext">The request context.</param>
        /// <param name="responseContext">The response context.</param>
        public static void Run(string apidCode, JavaScriptRequestContext requestContext, JavaScriptResponseContext responseContext)
        {
            var jsEngine = CreateEngine();
            var runMethod = jsEngine.Execute(apidCode).GetValue("run");
            runMethod.Invoke(JsValue.FromObject(jsEngine, requestContext), JsValue.FromObject(jsEngine, responseContext));
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>Javascript engine</returns>
        private static Engine CreateEngine()
        {
            return new Engine(option =>
            {
                option.AllowDebuggerStatement(false);
                option.Strict(true);
                option.LimitRecursion(10);
                option.MaxStatements(100);
                option.TimeoutInterval(TimeSpan.FromSeconds(1));
            });
        }
    }
}
