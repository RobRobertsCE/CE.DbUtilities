using CE.DbConnect.Models;
using CE.PfsConnect;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DevDbConnection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var pfsFile = PfsConfigurationFactory.GetPfsConfiguraiton();

                var valid = pfsFile.IsFileValid();

                Console.WriteLine(pfsFile.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var context = new CE.DbConnect.Data.DevelopmentContext();

                //var server = context.Servers.FirstOrDefault(s => s.Name == "SQLEXPRESS2K12" & s.MachineInstance.Name == "L-CE1358");

                //var db1 = new DatabaseInstance()
                //{
                //    DatabaseId = Guid.NewGuid(),
                //    Name = "CenterEdge",
                //    ServerId = server.ServerId,
                //    Version = "19.1"
                //};

                //context.Databases.Add(db1);

                //var db2 = new DatabaseInstance()
                //{
                //    DatabaseId = Guid.NewGuid(),
                //    Name = "sz",
                //    ServerId = server.ServerId,
                //    Version = "18.2"
                //};

                //context.Databases.Add(db2);

                //context.SaveChanges();

                ////foreach (var db in context.Databases)
                ////{
                ////    Console.WriteLine(db.Name);
                ////}
                //var machine = context.Machines.FirstOrDefault(s => s.Name == "ROB-PC");


                //var machine = new MachineInstance()
                //{
                //    MachineId = Guid.NewGuid(),
                //    Name = "L-CE1358"
                //};
                //context.Machines.Add(machine);

                //var newServer = new ServerInstance()
                //{
                //    ServerId = Guid.NewGuid(),
                //    Name = "SQLEXPRESS2K12",
                //    SqlVersion = "20012",
                //    MachineId = machine.MachineId
                //};

                //context.Servers.Add(newServer);


                ////var server = context.Servers.FirstOrDefault(s => s.Name == "SQLEXPRESS2K12");
                //var newCredentials = new SqlCredentials()
                //{
                //    SqlCredentialId = Guid.NewGuid(),
                //    UserId = "sa",
                //    Password = "sql1234!",
                //    UseEncryption = false,
                //    UseIntegratedSecurity = false,
                //    ServerId = newServer.ServerId
                //};

                //context.Credentials.Add(newCredentials);

                //var newServer2 = new ServerInstance()
                //{
                //    ServerId = Guid.NewGuid(),
                //    Name = "SQLEXPRESS2K17",
                //    SqlVersion = "20017",
                //    MachineId = machine.MachineId
                //};

                //context.Servers.Add(newServer2);


                ////var server = context.Servers.FirstOrDefault(s => s.Name == "SQLEXPRESS");
                //var newCredentials2 = new SqlCredentials()
                //{
                //    SqlCredentialId = Guid.NewGuid(),
                //    UserId = "sa",
                //    Password = "sql1234!",
                //    UseEncryption = false,
                //    UseIntegratedSecurity = false,
                //    ServerId = newServer2.ServerId
                //};

                //context.Credentials.Add(newCredentials2);


                //context.SaveChanges();

                //var newMachine = new MachineInstance()
                //{
                //    MachineId = Guid.NewGuid(),
                //    Name = "ROB-PC"
                //};

                //context.Machines.Add(newMachine);

                //context.SaveChanges();

                //var newServer = new ServerInstance()
                //{
                //    ServerId = Guid.NewGuid(),
                //    Name = "SQLEXPRESS",
                //    SqlVersion = "2008R2",
                //    MachineId = newMachine.MachineId
                //};

                //context.Servers.Add(newServer);

                //context.SaveChanges();


                //var newdb = new DatabaseInstance()
                //{
                //    DatabaseId = Guid.NewGuid(),
                //    Name = "SQLEXPRESS",
                //    Version = "10",
                //    ServerId = newServer.ServerId
                //};

                //context.Databases.Add(newdb);

                //context.SaveChanges();

                foreach (var machine in context.Machines.Include("ServerInstances").Include("ServerInstances.Credentials").Include("ServerInstances.Databases"))
                {
                    Console.WriteLine($"Machine - {machine.Name}");

                    foreach (var server in machine.ServerInstances)
                    {
                        Console.WriteLine($"    Server - {server.Name}");
                        Console.WriteLine($"    *** Databases ***");
                        foreach (var db in server.Databases)
                        {
                            Console.WriteLine($"        UserId - {db.SqlCredential?.UserId}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
    }
}
