var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("checklistdb");

builder.AddProject<Projects.CheckList_Web>("web")
    .WithReference(sql)
    .WaitFor(sql);

builder.Build().Run();
