﻿using Fluxor.UnitTests.DependencyInjectionTests.EffectDiscoveryTests.DiscoverEffectsWithActionInAttributeTests.SupportFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Fluxor.UnitTests.DependencyInjectionTests.EffectDiscoveryTests.DiscoverEffectsWithActionInAttributeTests
{
	public class DiscoverEffectsWithActionInAttributeTests
	{
		private readonly IServiceProvider ServiceProvider;
		private readonly IStore Store;
		private readonly IState<TestState> State;

		[Fact]
		public void WhenActionIsDispatched_ThenEffectWithActionInMethodSignatureIsExecuted()
		{
			Store.Dispatch(new TestAction());
			// 4 effects.
			// Static & Instance
			// Assembly scanned types + Explicitly specfied types
			Assert.Equal(4, State.Value.Count);
		}

		public DiscoverEffectsWithActionInAttributeTests()
		{
			var services = new ServiceCollection();
			services.AddFluxor(x => x
				.ScanAssemblies(GetType().Assembly)
				.ScanTypes(
					typeof(TypesThatShouldOnlyBeScannedExplicitly.InstanceTestEffects),
					typeof(TypesThatShouldOnlyBeScannedExplicitly.StaticTestEffects)
				)
				.AddMiddleware<IsolatedTests>());

			ServiceProvider = services.BuildServiceProvider();
			Store = ServiceProvider.GetRequiredService<IStore>();
			State = ServiceProvider.GetRequiredService<IState<TestState>>();

			Store.InitializeAsync().Wait();
		}
	}
}
