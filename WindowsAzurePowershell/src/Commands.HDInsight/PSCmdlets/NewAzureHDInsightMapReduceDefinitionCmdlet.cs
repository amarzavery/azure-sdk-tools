﻿// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License.  You may obtain a copy
// of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
// WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
// 
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.PSCmdlets
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.BaseCommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.ServiceLocation;

    /// <summary>
    ///     Represents the New-AzureHDInsightConfig Power Shell Cmdlet.
    /// </summary>
    [Cmdlet(VerbsCommon.New, AzureHdInsightPowerShellConstants.AzureHDInsightMapReduceJobDefinition)]
    public class NewAzureHDInsightMapReduceDefinitionCmdlet : AzureHDInsightCmdlet, INewAzureHDInsightMapReduceJobDefinitionBase
    {
        private readonly INewAzureHDInsightMapReduceJobDefinitionCommand command;

        /// <summary>
        ///     Initializes a new instance of the NewAzureHDInsightMapReduceDefinitionCmdlet class.
        /// </summary>
        public NewAzureHDInsightMapReduceDefinitionCmdlet()
        {
            this.command = ServiceLocator.Instance.Locate<IAzureHDInsightCommandFactory>().CreateNewMapReduceDefinition();
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The arguments for the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasArguments)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public string[] Arguments
        {
            get { return this.command.Arguments; }
            set { this.command.Arguments = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = true, HelpMessage = "The class name to use for the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasClassName)]
        public string ClassName
        {
            get { return this.command.ClassName; }
            set { this.command.ClassName = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The parameters for the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasParameters)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public Hashtable Defines
        {
            get { return this.command.Defines; }
            set { this.command.Defines = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The resources for the jobDetails.")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public string[] Files
        {
            get { return this.command.Files; }
            set { this.command.Files = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = true, HelpMessage = "The jar file to use for the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasJarFile)]
        public string JarFile
        {
            get { return this.command.JarFile; }
            set { this.command.JarFile = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The name of the jobDetails.")]
        [Alias(AzureHdInsightPowerShellConstants.AliasJobName)]
        public string JobName
        {
            get { return this.command.JobName; }
            set { this.command.JobName = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The lib jars for the jobDetails.")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Need collections for input parameters")]
        public string[] LibJars
        {
            get { return this.command.LibJars; }
            set { this.command.LibJars = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false, HelpMessage = "The output location to use for the jobDetails.")]
        public string StatusFolder
        {
            get { return this.command.StatusFolder; }
            set { this.command.StatusFolder = value; }
        }

        /// <summary>
        ///     Finishes the execution of the cmdlet by writing out the config object.
        /// </summary>
        protected override void EndProcessing()
        {
            this.command.EndProcessing().Wait();
            foreach (AzureHDInsightMapReduceJobDefinition output in this.command.Output)
            {
                this.WriteObject(output);
            }
            this.WriteDebugLog();
        }

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            this.command.Cancel();
        }
    }
}
