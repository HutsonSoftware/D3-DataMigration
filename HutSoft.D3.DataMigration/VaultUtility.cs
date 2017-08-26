using Autodesk.Connectivity.WebServicesTools;
using System;

namespace HutSoft.D3.DataMigration
{
    internal class VaultUtility
    {
        private Settings _settings;

        internal VaultUtility(Settings settings)
        {
            _settings = settings;
        }

        internal Settings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        internal WebServiceManager LoginToVault()
        {
            return LoginToVault(_settings.VaultServer, _settings.VaultInstance, _settings.VaultUserName, _settings.VaultPassword);
        }

        internal WebServiceManager LoginToVault(string vaultServer, string vaultInstance, string vaultUserName, string vaultPassword)
        {
            WebServiceManager webServiceManager;
            try
            {
                webServiceManager = new WebServiceManager(new UserPasswordCredentials(vaultServer, vaultInstance, vaultUserName, vaultPassword, false));
                if (webServiceManager != null)
                    webServiceManager.ReSignIn = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return webServiceManager;
        }
    }
}
