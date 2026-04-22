var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("checklistdb");

var api = builder.AddProject<Projects.CheckList_Api>("api")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.CheckList_Web>("web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
