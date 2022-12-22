// See https://aka.ms/new-console-template for more information

using RaylibTest;

var app = new App();

app
    .AddPlugin(new DefaultPlugin())
    .AddPlugin(new RenderPlugin())
    .AddPlugin(new AnimationPlugin())
    .AddPlugin(new GamePlugin())
    .Run();