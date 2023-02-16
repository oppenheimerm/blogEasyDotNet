using BE.DataStore.EFCore;
using BE.DataStore.EFCore.Repositories;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.UseCase.Image;
using BE.UseCases.UseCase.Post;
using BE.UseCases.UseCase.PostTag;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().
    AddRazorPagesOptions(options => {
        /* Friendly Routes - browse blog/some-slug-here */
        options.Conventions.AddPageRoute("/blog/slug", "blog/{slug}");

    });

//  Must go before  services.AddMvc() or services.AddControllersWithViews()!!
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<BEDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//builder.Services.AddSingleton<IBlogUserServices, BlogUserServices>();


//  Repositories
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();


//  Usercases
builder.Services.AddTransient<IAddCoverPhotoUseCase, AddCoverPhotoUseCase>();
builder.Services.AddTransient<IViewBlogEntryBySlug, ViewBlogEntryBySlug>();
builder.Services.AddTransient<IViewBlogEntiresByFilterUseCase, ViewBlogEntiresByFilterUseCase>();
builder.Services.AddTransient<ICreatePostUseCase, CreatePostUseCase>();
builder.Services.AddTransient<IEditPostUseCase, EditPostUseCase>();
builder.Services.AddTransient<IViewBlogEntriesByTag, ViewBlogEntriesByTag>();
builder.Services.AddTransient<IViewBlogEntryById, ViewBlogEntryById>();
builder.Services.AddTransient<IUpdatePostTagsUseCase, UpdatePostTagsUseCase>();
builder.Services.AddTransient<IDeletePostTagsUseCase, DeletePostTagsUseCase>();
builder.Services.AddTransient<IAddPostTagsUseCase, AddPostTagsUseCase>();
builder.Services.AddTransient<IDeleteCoverPhotoUseCase, DeleteCoverPhotoUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();

}
else
{
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<BE.DataStore.EFCore.BEDbContext>();
	//   takes no action if a database for the context exists. If no
	//   database exists, it creates the database and schema. 
	context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
