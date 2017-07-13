﻿using CompiledHandlebars.Compiler;
using CompiledHandlebars.CompilerTests.Helper;
using CompiledHandlebars.CompilerTests.TestViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiledHandlebars.CompilerTests
{
	[TestClass]
	public class PartialTests : CompilerTestBase
	{
		private const string _marsModel = "{{model CompiledHandlebars.CompilerTests.TestViewModels.MarsModel}}";
		private const string _starModel = "{{model CompiledHandlebars.CompilerTests.TestViewModels.StarModel}}";
		private const string _selfRefModel = "{{model CompiledHandlebars.CompilerTests.TestViewModels.SelfReferencingViewModel}}";
		static PartialTests()
		{
			assemblyWithCompiledTemplates = CompileTemplatesToAssembly(typeof(PartialTests));
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("BasicPartialTest1", "{{model System.String}}{{this}}")]
		[RegisterHandlebarsTemplate("BasicPartialTest2", "{{model System.String}}{{> BasicPartialTest1 this}}")]
		[RegisterHandlebarsTemplate("BasicPartialTest3", "{{#each Planets}}{{> BasicPartialTest1 Name}}{{/each}}", _starModel)]
		public void BasicPartialTest()
		{
			ShouldRender("BasicPartialTest2", "Mars", "Mars");
			ShouldRender("BasicPartialTest3", CelestialBodyFactory.CreateSolarSystem(), "MercuryVenusEarthMars");
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("NullParameterPartialTest1", "{{model System.String}}{{> BasicPartialTest1 this}}")]
		public void NullParameterPartialTest()
		{
			ShouldRender("NullParameterPartialTest1", default(string), "");
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("ImpliedThisParameterTest1", "{{model System.String}}{{> BasicPartialTest1}}")]
		public void ImpliedThisParameterTest()
		{
			ShouldRender("ImpliedThisParameterTest1", "Mars", "Mars");
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("SelfReferencingPartialTest1", "{{#if Name}}{{Name}}{{/if}}{{#if Child}}{{> SelfReferencingPartialTest1 Child}}{{/if}}", _selfRefModel)]
		[RegisterHandlebarsTemplate("SelfReferencingPartialTest2", "{{#if Name}}{{Name}}{{/if}}{{#if Child}}{{> CompilerTests.SelfReferencingPartialTest2 Child}}{{/if}}", _selfRefModel)]
		[RegisterHandlebarsTemplate("SelfReferencingPartialTest3", "{{#if Name}}{{Name}}{{/if}}{{#if Child}}{{> Tests.SelfReferencingPartialTest3 Child}}{{/if}}", _selfRefModel, false)]
		public void SelfReferencingPartialTest()
		{
			ShouldRender("SelfReferencingPartialTest1", SelfReferencingViewModelFactory.Create(), "ParentChild");
			ShouldRender("SelfReferencingPartialTest2", SelfReferencingViewModelFactory.Create(), "ParentChild");
			ShouldRaiseError("SelfReferencingPartialTest3", HandlebarsTypeErrorKind.UnknownPartial);
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("UnknownPartialTest1", "{{> NoSuchTemplate this}}", _marsModel, false)]
		public void UnkownPartialTest()
		{
			ShouldRaiseError("UnknownPartialTest1", HandlebarsTypeErrorKind.UnknownPartial);
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("_PartialWithUnderscoreTest1", "{{Name}}", _marsModel)]
		[RegisterHandlebarsTemplate("PartialWithUnderscoreTest2", "{{> _PartialWithUnderscoreTest1}}", _marsModel)]
		[RegisterHandlebarsTemplate("PartialWithUnderscoreTest3", "{{> CompilerTests._PartialWithUnderscoreTest1 this}}", _marsModel)]
		public void PartialWithUnderscoreTest()
		{
			ShouldRender("PartialWithUnderscoreTest2", MarsModelFactory.CreateFullMarsModel(), "Mars");
			ShouldRender("PartialWithUnderscoreTest3", MarsModelFactory.CreateFullMarsModel(), "Mars");
		}

		[TestMethod]
		[RegisterHandlebarsTemplate("NamespacedPartialTest1", "{{> CompilerTests.BasicPartialTest1 Name}}", _marsModel)]
		public void NamespacedPartialTest()
		{
			ShouldRender("NamespacedPartialTest1", MarsModelFactory.CreateFullMarsModel(), "Mars");
		}
	}
}
