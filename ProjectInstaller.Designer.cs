namespace TankService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.TankInvent = new System.ServiceProcess.ServiceProcessInstaller();
            this.TankServiceInvent = new System.ServiceProcess.ServiceInstaller();
            // 
            // TankInvent
            // 
            this.TankInvent.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.TankInvent.Password = null;
            this.TankInvent.Username = null;
            this.TankInvent.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // TankServiceInvent
            // 
            this.TankServiceInvent.Description = "My service";
            this.TankServiceInvent.DisplayName = "TankInvent";
            this.TankServiceInvent.ServiceName = "Tank";
            this.TankServiceInvent.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.TankServiceInvent_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TankInvent,
            this.TankServiceInvent});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TankInvent;
        private System.ServiceProcess.ServiceInstaller TankServiceInvent;
    }
}