using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DevTestProject.Utils
{
    public static class Constants
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        public const string  EMPLOYEES_TABLE = "[dbo].[employees]";
        public const string  PROJECT_COOPERATION_TABLE = "[dbo].[project_cooperation]";
        public const string  PROJECTS_TABLE = "[dbo].[projects]";
        public const string  TEAMS_TABLE = "[dbo].[teams]";
        public const string  WORK_ITEMS_TABLE = "[dbo].[work_items]";
    }
}