using FurryFriends.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FurryFriends.API.Services
{
    public class ProductExpirationService : BackgroundService
    {
        private readonly ILogger<ProductExpirationService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Check every hour

        public ProductExpirationService(
            ILogger<ProductExpirationService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Product Expiration Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        
                        // Get all active products that have expired
                        var now = DateTime.Now;
                        var expiredProducts = await dbContext.SanPhams
                            .Where(p => p.TrangThai && p.HanSuDung.HasValue && p.HanSuDung.Value <= now)
                            .ToListAsync(stoppingToken);

                        if (expiredProducts.Any())
                        {
                            _logger.LogInformation($"Found {expiredProducts.Count} expired products. Updating status...");
                            
                            // Update status to inactive
                            foreach (var product in expiredProducts)
                            {
                                product.TrangThai = false;
                                _logger.LogInformation($"Product {product.TenSanPham} (ID: {product.SanPhamId}) has expired and has been deactivated.");
                            }
                            
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking for expired products");
                }

                // Wait for the next check interval
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
