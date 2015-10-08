using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Web;

/// <summary>
/// Summary description for PowershellRunner
/// </summary>
public class PowershellRunner
{
	public PowershellRunner()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static Collection<PSObject> run(string scriptFile, List<string> parameters)
    {
        string paramName;
        string paramValue;
        RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
        Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
        runspace.Open();
        
        RunspaceInvoke scriptIvoker = new RunspaceInvoke(runspace);
        Pipeline pipeline = runspace.CreatePipeline();

        Command cmd = new Command(scriptFile);
        //CommandParameter param = new CommandParameter("-ComputerName", "Powerhouse");
        //cmd.Parameters.Add(param);
        foreach (string scriptParameter in parameters)
        {
            paramName = scriptParameter.Split(' ')[0];
            paramValue = scriptParameter.Split(' ')[1];
            cmd.Parameters.Add(paramName, paramValue);
        }
        
        pipeline.Commands.Add(cmd);

        return pipeline.Invoke();
    }
}