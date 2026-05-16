// PulseHub.Application/DependencyInjection.cs

using Microsoft.Extensions.DependencyInjection;
using PulseHub.Application.Orders.Commands;
using PulseHub.Application.Orders.Queries;
using PulseHub.Application.Products.Commands;
using PulseHub.Application.Products.Queries;
using PulseHub.Application.Categories.Commands;
using PulseHub.Application.Categories.Queries;
using PulseHub.Application.Users.Commands;
using PulseHub.Application.Users.Queries;

namespace PulseHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Users
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUsersHandler>();

        // Orders
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<AddOrderItemHandler>();
        services.AddScoped<ConfirmOrderHandler>();
        services.AddScoped<CancelOrderHandler>();
        services.AddScoped<GetOrdersHandler>();
        services.AddScoped<GetOrderByIdHandler>();

        // Products
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<GetProductsHandler>();
        services.AddScoped<GetProductByIdHandler>();

        // Categories
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<GetCategoriesHandler>();

        return services;
    }
}
