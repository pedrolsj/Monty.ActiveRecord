using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace Monty.Log
{
    /// <summary>
    /// Log Manager
    /// </summary>
    public static class LogManager
    {
        #region Contructors

        /// <summary>
        /// Initializes the <see cref="LogManager"/> class.
        /// </summary>
        static LogManager()
        {
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Logs the method.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="method">The method.</param>
        /// <param name="parametersNames">The parameters names.</param>
        /// <param name="parametersValue">The parameters value.</param>
        public static void LogMethod(string section, LogCategory category, String method, string parametersNames, object[] parametersValue)
        {
            LogManager.LogMethod(section, category, method, parametersNames.Split(','), parametersValue, null);
        }

        /// <summary>
        /// Logs the method.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="method">The method.</param>
        /// <param name="parametersNames">The parameters names.</param>
        /// <param name="parametersValue">The parameters value.</param>
        /// <param name="exception">The exception.</param>
        public static void LogMethod(string section, LogCategory category, String method, string parametersNames, object[] parametersValue, Exception exception)
        {
            LogManager.LogMethod(section, category, method, parametersNames.Split(','), parametersValue, exception);
        }

        /// <summary>
        /// Logs the method.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="method">The method.</param>
        /// <param name="parametersNames">The parameters names.</param>
        /// <param name="parametersValue">The parameters value.</param>
        public static void LogMethod(string section, LogCategory category, String method, string[] parametersNames, object[] parametersValue)
        {
            LogManager.LogMethod(section, category, method, parametersNames, parametersValue, null);
        }

        /// <summary>
        /// Logs the method.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="method">The method.</param>
        /// <param name="parametersNames">The parameters names.</param>
        /// <param name="parametersValue">The parameters value.</param>
        /// <param name="exception">The exception.</param>
        public static void LogMethod(string section, LogCategory category, String method, string[] parametersNames, object[] parametersValue, Exception exception)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Error when tries to execute {0}(", method);
            for (int i = 0; i < parametersNames.Length; i++)
            {
                builder.AppendFormat("{0}: [{1}]", parametersNames[i], parametersValue[i]);

                if (i != parametersNames.Length - 1)
                    builder.Append(",");
            }
            builder.Append(");");

            LogManager.Log(section, category, builder.ToString(), exception);
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        public static void Log(string section, LogCategory category, string message)
        {
            message = String.Format("Message: {0}",
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(string section, LogCategory category, string message, Exception exception)
        {
            message = String.Format("Message: {0}",
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message, exception);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message, exception);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message, exception);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message, exception);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message, exception);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="usr">The usr.</param>
        /// <param name="message">The message.</param>
        public static void Log(string section, LogCategory category, string usr, string message)
        {
            message = String.Format("User: {0}, \n Message: {1}",
                usr,
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="category">The category.</param>
        /// <param name="usr">The usr.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(string section, LogCategory category, string usr, string message, Exception exception)
        {
            message = String.Format("User: {0}, \n Message: {1}",
                usr,
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message, exception);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message, exception);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message, exception);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message, exception);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message, exception);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="contextClass">The context class.</param>
        /// <param name="contextMethod">The context method.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        public static void Log(string section, Type contextClass, string contextMethod, LogCategory category, string message)
        {
            message = String.Format("Type: [{0}], \n Assembly: [{1}], \n Method: [{2}], \n Message: {3}",
                contextClass.FullName,
                contextClass.Assembly.FullName,
                contextMethod,
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="contextClass">The context class.</param>
        /// <param name="contextMethod">The context method.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(string section, Type contextClass, string contextMethod, LogCategory category, string message, Exception exception)
        {
            message = String.Format("Type: [{0}], \n Assembly: [{1}], \n Method: [{2}], \n Message: {3}",
                contextClass.FullName,
                contextClass.Assembly.FullName,
                contextMethod,
                message
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message, exception);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message, exception);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message, exception);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message, exception);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message, exception);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="contextClass">The context class.</param>
        /// <param name="contextMethod">The context method.</param>
        /// <param name="category">The category.</param>
        /// <param name="usr">The usr.</param>
        /// <param name="message">The message.</param>
        public static void Log(string section, Type contextClass, string contextMethod, LogCategory category, string usr, string message)
        {
            message = String.Format("Type: [{2}], \n Assembly: [{3}], \n Method: [{4}], \n User: {0}, \n Message: {1}",
                usr,
                message,
                contextClass.FullName,
                contextClass.Assembly.FullName,
                contextMethod
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="contextClass">The context class.</param>
        /// <param name="contextMethod">The context method.</param>
        /// <param name="category">The category.</param>
        /// <param name="usr">The usr.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(string section, Type contextClass, string contextMethod, LogCategory category, string usr, string message, Exception exception)
        {
            message = String.Format("Type: [{2}], \n Assembly: [{3}], \n Method: [{4}], \n User: {0}, \n Message: {1}",
                usr,
                message,
                contextClass.FullName,
                contextClass.Assembly.FullName,
                contextMethod
            );

            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message, exception);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message, exception);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message, exception);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message, exception);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message, exception);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="contextClass">The context class.</param>
        /// <param name="contextMethod">The context method.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Log(string section, Type contextClass, string contextMethod, LogCategory category, string message, object[] parameters)
        {
            string paramList = "";
            foreach (object param in parameters)
                paramList += param.ToString() + ",";

            message = String.Format("Type: [{0}], \n Assembly: [{1}], \n Method: [{2}( {4} )], \n Message: {3}",
                contextClass.FullName,
                contextClass.Assembly.FullName,
                contextMethod,
                message,
                paramList
            );


            switch (category)
            {
                case LogCategory.Info:
                    log4net.LogManager.GetLogger(section).Info(message);
                    break;
                case LogCategory.Warn:
                    log4net.LogManager.GetLogger(section).Warn(message);
                    break;
                case LogCategory.Debug:
                    log4net.LogManager.GetLogger(section).Debug(message);
                    break;
                case LogCategory.Error:
                    log4net.LogManager.GetLogger(section).Error(message);
                    break;
                case LogCategory.Fatal:
                    log4net.LogManager.GetLogger(section).Fatal(message);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
