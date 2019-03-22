#tool nuget:?package=Cake.Bakery&version=0.4.1

Task("Default")
    .Does(() =>
{
    // Information($"Hello! {DateTime.Now}");
    foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()){
        Information(asm.FullName);
    }
});

RunTarget("Default");