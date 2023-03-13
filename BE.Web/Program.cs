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
builder.Services.AddScoped<IPhotoFileRepository, PhotoFileRepository>();
builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();
builder.Services.AddScoped<IFolderEntityRepository, FolderEntityRepository>();
builder.Services.AddScoped<IPostImageEntityRepository, PostImageEntityRepository>();



//  Usercases
builder.Services.AddTransient<IAddCoverPhotoUseCase, AddCoverPhotoUseCase>();
builder.Services.AddTransient<IViewBlogEntryBySlug, ViewBlogEntryBySlug>();
builder.Services.AddTransient<IViewBlogEntiresByFilterUseCase, ViewBlogEntiresByFilterUseCase>();
builder.Services.AddTransient<ICreatePostUseCase, CreatePostUseCase>();
builder.Services.AddTransient<IEditPostUseCase, EditPostUseCase>();
builder.Services.AddTransient<IViewBlogEntriesByTag, ViewBlogEntriesByTag>();
builder.Services.AddTransient<IViewBlogEntryById, ViewBlogEntryById>();
builder.Services.AddTransient<IDeletePostTagsUseCase, DeletePostTagsUseCase>();
builder.Services.AddTransient<IDeleteCoverPhotoUseCase, DeleteCoverPhotoUseCase>();
builder.Services.AddTransient<IAddFolderEntityUseCase, AddFolderEntityUseCase>();
builder.Services.AddTransient<IAddFolderUseCase, AddFolderUseCase>();
builder.Services.AddTransient<IAddPostImageEntityUseCase, AddPostImageEntityUseCase>();
builder.Services.AddTransient<IDeleteCoverPhotoDbEntityUseCase, DeleteCoverPhotoDbEntityUseCase>();
builder.Services.AddTransient<IAddPhotoUseCase, AddPhotoUseCase>();
builder.Services.AddTransient<IGetFolderEntityUseCase, GetFolderEntityUseCase>();
builder.Services.AddTransient<IDeleteCoverPhotoFileUseCase, DeleteCoverPhotoFileUseCase>();
builder.Services.AddTransient<IAddCoverPhotoUseCase, AddCoverPhotoUseCase>();
builder.Services.AddTransient<IPurgPostFilesUseCase, PurgPostFilesUseCase>();
builder.Services.AddTransient<IDeletePostUseCase, DeletePostUseCase>();


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

	//	A database that is created by EnsureCreated can't be updated by using migrations.
	//	context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//  make use of a pages that you create to generate error response,
//  i.e 404, 500...
app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
