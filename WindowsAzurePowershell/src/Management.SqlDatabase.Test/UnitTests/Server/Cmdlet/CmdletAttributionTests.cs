﻿// ----------------------------------------------------------------------------------
//
// Copyright 2011 Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Management.Automation;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Management.CloudService.Model;
using Microsoft.WindowsAzure.Management.CloudService.Test;
using Microsoft.WindowsAzure.Management.SqlDatabase.Server.Cmdlet;
using Microsoft.WindowsAzure.Management.SqlDatabase.Test.UnitTest;
using Microsoft.WindowsAzure.Management.Test.Stubs;
using System;
using Microsoft.WindowsAzure.Management.SqlDatabase.Firewall.Cmdlet;

namespace Microsoft.WindowsAzure.Management.SqlDatabase.Test.UnitTests.Server.Cmdlet
{
    /// <summary>
    /// These tests prevent regression in parameter validation attributes.
    /// </summary>
    [TestClass]
    public class CmdletAttributionTests : TestBase
    {
        [TestInitialize]
        public void SetupTest()
        {
        }

        [TestMethod]
        public void GetAzureSqlDatabaseServerAttributeTest()
        {
            Type cmdlet = typeof(GetAzureSqlDatabaseServer);
            CheckConfirmImpact(cmdlet, ConfirmImpact.None);
        }

        [TestMethod]
        public void NewAzureSqlDatabaseServerAttributeTest()
        {
            Type cmdlet = typeof(NewAzureSqlDatabaseServer);
            CheckConfirmImpact(cmdlet, ConfirmImpact.Low);
        }

        [TestMethod]
        public void RemoveAzureSqlDatabaseServerAttributeTest()
        {
            Type cmdlet = typeof(RemoveAzureSqlDatabaseServer);
            CheckConfirmImpact(cmdlet, ConfirmImpact.High);
        }

        [TestMethod]
        public void SetAzureSqlDatabasePasswordAttributeTest()
        {
            Type cmdlet = typeof(SetAzureSqlDatabasePassword);
            CheckConfirmImpact(cmdlet, ConfirmImpact.Medium);
        }

        [TestMethod]
        public void GetAzureSqlDatabaseFirewallRuleAttributeTest()
        {
            Type cmdlet = typeof(GetAzureSqlDatabaseFirewallRule);
            CheckConfirmImpact(cmdlet, ConfirmImpact.None);
        }

        [TestMethod]
        public void NewAzureSqlDatabaseFirewallRuleAttributeTest()
        {
            Type cmdlet = typeof(NewAzureSqlDatabaseFirewallRule);
            CheckConfirmImpact(cmdlet, ConfirmImpact.Low);
        }

        [TestMethod]
        public void RemoveAzureSqlDatabaseFirewallRuleAttributeTest()
        {
            Type cmdlet = typeof(RemoveAzureSqlDatabaseFirewallRule);
            CheckConfirmImpact(cmdlet, ConfirmImpact.Medium);
        }

        #region Helpers

        private static void CheckConfirmImpact(Type cmdlet, ConfirmImpact confirmImpact)
        {
            object[] cmdletAttributes = cmdlet.GetCustomAttributes(typeof(CmdletAttribute), true);
            Assert.AreEqual(cmdletAttributes.Length, 1);
            CmdletAttribute attribute = (CmdletAttribute)cmdletAttributes[0];
            Assert.AreEqual(attribute.ConfirmImpact, confirmImpact);
        }

        #endregion
    }
}
