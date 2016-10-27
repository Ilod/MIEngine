// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

namespace MIDebugLauncher
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.IO;
  using System.Threading.Tasks;
  using Microsoft.VisualStudio.ProjectSystem;
  using Microsoft.VisualStudio.ProjectSystem.Debuggers;
  using Microsoft.VisualStudio.ProjectSystem.Utilities;
  using Microsoft.VisualStudio.ProjectSystem.Utilities.DebuggerProviders;
  using Microsoft.VisualStudio.ProjectSystem.VS.Debuggers;
  using Microsoft.VisualStudio.Shell.Interop;
  using MICore.Xml.LaunchOptions;

  /// <summary>
  /// A Visual C++ extension that launches a custom debugger.
  /// </summary>
  [ExportDebugger("MocheDebugger")]  // Keep this string in sync with the one in your debugger's XAML file.
  [AppliesTo(ProjectCapabilities.VisualC)]
  public class MocheDebuggerLauncher : DebugLaunchProviderBase
  {
    [ImportingConstructor]
    public MocheDebuggerLauncher(ConfiguredProject configuredProject)
        : base(configuredProject)
    {
    }

    /// <summary>
    /// Gets project properties that the debugger needs to launch.
    /// </summary>
    [Import]
    private Rules.RuleProperties DebuggerProperties { get; set; }

    public override async Task<bool> CanLaunchAsync(DebugLaunchOptions launchOptions)
    {
      var properties = await this.DebuggerProperties.GetMocheDebuggerPropertiesAsync();
      string commandValue = await properties.LocalDebuggerCommand.GetEvaluatedValueAtEndAsync();
      string debuggerPath = await properties.LocalDebuggerPath.GetEvaluatedValueAtEndAsync();
      return !string.IsNullOrEmpty(commandValue) && !string.IsNullOrEmpty(debuggerPath);
    }

    public override async Task LaunchAsync(DebugLaunchOptions launchOptions)
    {
      // The properties that are available via DebuggerProperties are determined by the property XAML files in your project.
      var debuggerProperties = await this.DebuggerProperties.GetMocheDebuggerPropertiesAsync();
      LocalLaunchOptions llo = new LocalLaunchOptions();
      llo.ExePath = await debuggerProperties.LocalDebuggerCommand.GetEvaluatedValueAtEndAsync();
      llo.ExeArguments = await debuggerProperties.LocalDebuggerCommandArguments.GetEvaluatedValueAtEndAsync();
      llo.WorkingDirectory = await debuggerProperties.LocalDebuggerWorkingDirectory.GetEvaluatedValueAtEndAsync();
      llo.MIDebuggerPath = await debuggerProperties.LocalDebuggerPath.GetEvaluatedValueAtEndAsync();
      MIMode miMode;
      if (Enum.TryParse(await debuggerProperties.LocalDebuggerType.GetEvaluatedValueAtEndAsync(), out miMode))
      {
        llo.MIMode = miMode;
        llo.MIModeSpecified = true;
      }
      if (llo.ExePath == null)
      {
        var generalProperties = await this.DebuggerProperties.GetConfigurationGeneralPropertiesAsync();
        llo.ExePath = await generalProperties.TargetPath.GetEvaluatedValueAtEndAsync();
      }

      IVsDebugger4 debugger = (IVsDebugger4)ServiceProvider.GetService(typeof(IVsDebugger));
      VsDebugTargetInfo4[] debugTargets = new VsDebugTargetInfo4[1];
      debugTargets[0].dlo = (uint)DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
      debugTargets[0].bstrExe = llo.MIDebuggerPath;
      using (MemoryStream s = new MemoryStream())
      {
        new System.Xml.Serialization.XmlSerializer(typeof(LocalLaunchOptions)).Serialize(s, llo);
        s.Flush();
        s.Seek(0, SeekOrigin.Begin);
        debugTargets[0].bstrOptions = await new StreamReader(s).ReadToEndAsync();
      }
      debugTargets[0].guidLaunchDebugEngine = Microsoft.MIDebugEngine.EngineConstants.EngineId;
      VsDebugTargetProcessInfo[] processInfo = new VsDebugTargetProcessInfo[debugTargets.Length];

      debugger.LaunchDebugTargets4(1, debugTargets, processInfo);
    }

    public override Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions launchOptions)
    {
      throw new NotImplementedException();
    }
  }
}
