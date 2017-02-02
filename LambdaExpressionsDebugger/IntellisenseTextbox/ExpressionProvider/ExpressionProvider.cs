using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace IntellisenseTextbox
{
    class ExpressionProvider
    {
        public ExpressionProvider(ContextProvider context)
        {            
            m_context = context;
            m_compiler = new ExpressionCompiler(m_context);
        }

        private Expression<Action> m_lastSuccessfullExpression;
        private ExpressionCompiler m_compiler;
        private Expression<Action> m_expression;
        private ExpressionErrorCollection m_errors = new ExpressionErrorCollection();

        private bool Success { get { return !m_errors.HasErrors; } }
                
        private ContextProvider m_context;

        public ExpressionResult GetExpression(string sourceCode)
        {
            ExpressionResult result = null;

            ClearExpression();
            ClearErrors();

            TryCompile(sourceCode);
            TryGetExpression();

            if (Success)
            {
                result = new ExpressionResult() { Expression = m_expression };
                m_lastSuccessfullExpression = m_expression;
            }
            else
                result = new ExpressionResult() { Expression = m_lastSuccessfullExpression, Errors = m_errors };

            return result;
        }

        private void ClearExpression()
        {
            m_expression = null;
        }

        private void ClearErrors()
        {
            m_errors = new ExpressionErrorCollection();
        }

        private void TryCompile(string sourceCode)
        {
            try
            {
                m_compiler.Compile(sourceCode);
            }
            catch (Exception exc)
            {
                m_errors.Errors.Add(new ExpressionError() { ErrorType = ErrorType.BuildError, Text = exc.Message });
            }
            if (m_compiler.Results.Errors.HasErrors)
            {
                foreach (var error in m_compiler.Results.Errors)
                    m_errors.Errors.Add(new ExpressionError() { ErrorType = ErrorType.BuildError, Text = error.ToString() });
            }
        }

        private void TryGetExpression()
        {
            if (m_compiler.Results.Errors.HasErrors)
                return;
            if (m_compiler.Results.CompiledAssembly == null)
                return;
            Type generatedType = m_compiler.Results.CompiledAssembly.GetType(ExpressionCompiler.GeneratedClassName);
            object generatedInstance = Activator.CreateInstance(generatedType);
            MethodInfo method = generatedType.GetMethod(ExpressionCompiler.GeneratedMethodName);
            try
            {
                m_expression = method.Invoke(generatedInstance, null) as Expression<Action>;
            }
            catch (Exception exc)
            {
                m_errors.Errors.Add(new ExpressionError() { ErrorType = ErrorType.ExecutionError, Text = exc.Message });
            }
        }
    }
}
