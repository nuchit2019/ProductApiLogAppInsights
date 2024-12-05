using Microsoft.ApplicationInsights;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, TelemetryClient telemetryClient)
        {
            _productRepository = productRepository;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            try
            {
                return await _productRepository.GetProductAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product");
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _productRepository.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _productRepository.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await _productRepository.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                _telemetryClient.TrackException(ex);
                throw;
            }
        }
    }
}