// See https://aka.ms/new-console-template for more information

using RaylibTest;

new App()
    .AddPlugin(new DefaultPlugin())
    .AddPlugin(new RenderPlugin())
    .AddPlugin(new AnimationPlugin())
    .AddPlugin(new GamePlugin())
    .Run();