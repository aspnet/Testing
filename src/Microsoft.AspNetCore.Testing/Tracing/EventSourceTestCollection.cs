namespace Microsoft.AspNetCore.Testing.Tracing
{
    // This file has to be public and has to actually live INSIDE the test assembly. Hence it is a shared-source file.
    // This file is NOT part of Microsoft.AspNetCore.Testing.Tracing's compilation sources, it is exported to NuGet brought in to each
    // see xunit bug: [TODO]
    [Xunit.CollectionDefinition("Microsoft.AspNetCore.Testing.Tracing.EventSourceTestCollection", DisableParallelization = true)]
    public class EventSourceTestCollection
    {
    }
}
