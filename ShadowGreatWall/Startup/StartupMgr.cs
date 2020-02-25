using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowGreatWall.Core;

namespace ShadowGreatWall.Startup
{
    class StartupMgr
    {
        #region [单例]
        private static readonly StartupMgr instance = new StartupMgr();

        public static StartupMgr Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region [变量]
        private Configuration config = null;
        #endregion

        #region [初始化]
        private StartupMgr()
        {
            config = Configuration.Load();
        }
        #endregion

        #region [接口]
        public void Start()
        { }
        #endregion

        #region [属性]
        public Configuration CurrentConfig
        {
            get
            {
                return config;
            }
        }
        #endregion
    }
}
