using System;
using TestFx;
using TestFx.FakeItEasy.TestExtensions;

// NOTE: currently called by FakeCreationTestExtension
//[assembly: UseTestExtension (typeof (FakeSetupTestExtension))]
[assembly: UseTestExtension (typeof (FakeCreationTestExtension))]
[assembly: UseTestExtension (typeof (FakeScopeTestExtension))]