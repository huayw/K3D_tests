﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Karamba.CrossSections;
using Karamba.Geometry;
using Karamba.Elements;
using Karamba.Loads;
using Karamba.Materials;
using Karamba.Supports;
using Karamba.Models;
using Karamba.Utilities;
using Karamba.Algorithms;


namespace KarambaCommon.Tests.CrossSections
{
    [TestFixture]
    public class ReadCrossSectionFromTable_tests
    {
#if ALL_TESTS
        [Test]
        public void ImperialUnits()
        {
            var k3d = new Toolkit();
            var logger = new MessageLogger();

            // make temporary changes to the the ini-file and units-conversion
            INIReader.ClearSingleton();
            UnitsConversionFactories.ClearSingleton();

            var ini = INIReader.Instance();
            ini.Values["UnitsSystem"] = "imperial";

            var resourcePath = Path.Combine(Utils.PluginPathExe(), @"..\..\Resources\");

            // get a cross section from the cross section table in the folder 'Resources'
            var crosecPath = Path.Combine(resourcePath, "CrossSectionValues.csv");
            CroSecTable inCroSecs = k3d.CroSec.ReadCrossSectionTable(crosecPath, out var info);
            var crosec_family = inCroSecs.crosecs.FindAll(x => x.family == "W");
            var crosec_initial = crosec_family.Find(x => x.name == "W12X26");

            // clear temporary changes to the the ini-file and units-conversion
            INIReader.ClearSingleton();
            UnitsConversionFactories.ClearSingleton();

            var cs = crosec_initial as CroSec_I;
            
            var m2feet = 3.28084;
            var height_feet = 0.31 * m2feet;
            var width_feet = 0.165 * m2feet;
            Assert.AreEqual(cs._height, height_feet, 1e-5);
            Assert.AreEqual(cs.lf_width, width_feet, 1e-5);

            var feet42inch4 = Math.Pow(12.0,4);
            var cm42inch4 = Math.Pow(1.0 / 2.54, 4);
            var Iyy_inch1 = 8491.12 * cm42inch4;
            var Iyy_inch2 = cs.Iyy * feet42inch4;
            Assert.AreEqual(Iyy_inch1, Iyy_inch2, 1e-1);
        }
#endif
    }
}
