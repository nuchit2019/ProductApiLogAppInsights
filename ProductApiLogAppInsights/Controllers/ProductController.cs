using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Logging;
using ProductApi.Models;
using ProductApi.Services;
using System.Text.Json;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly TelemetryClient _telemetryClient;
        private string processName = "";

        public ProductController(IProductService productService, ILogger<ProductController> logger, TelemetryClient telemetryClient)
        {
            _productService = productService;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            //=====================================================//
            // 1. START_PROCESS
            //=====================================================//

            processName = nameof(GetProduct) + $" with Id: {id}";
            TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.START_PROCESS, id);

            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................
                if (id <= 0)
                {
                    
                    processName = nameof(GetProduct) + $" Invalid product ID: {id}";
                    //=====================================================//
                    // 2. WARNING_PROCESS
                    //=====================================================//
                    TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.WARNING_PROCESS);
                }              


                // ..............................
                //*** Business logic
                // ..............................
                var product = await _productService.GetProductAsync(id);


                //=====================================================//
                // 3. SUCCESS_PROCESS
                //=====================================================// 
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.SUCCESS_PROCESS);

                return Ok(product);
            }
            catch (Exception ex)
            {
                //=====================================================//
                // 4. EXCEPTION_PROCESS
                //=====================================================//
                var detail = new { ProductID = id };
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.EXCEPTION_PROCESS, detail, ex);
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            //=====================================================//
            // 1. START_PROCESS
            //=====================================================//
            processName = nameof(GetAllProducts); 
            TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.START_PROCESS);
            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................

                //=====================================================//
                // 2. WARNING_PROCESS
                //=====================================================//
                //... 
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.WARNING_PROCESS);

                //=====================================================//
                // 3. SUCCESS_PROCESS
                //=====================================================//
                var products = await _productService.GetAllProductsAsync();               
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.SUCCESS_PROCESS, products);




                // Test exception
                //throw new Exception(string.Format(LoggingConstants.START_PROCESS, processName)+" Test exception for logging.");
                return Ok(products);
            }
            catch (Exception ex)
            {
                //=====================================================//
                // 3. EXCEPTION_PROCESS
                //=====================================================//
           
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.EXCEPTION_PROCESS, ex); 
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            //=====================================================//
            // 1. START_PROCESS
            //=====================================================//
            processName = nameof(AddProduct); 
            TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.START_PROCESS);

            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................

                //=====================================================//
                // 3. WARNING_PROCESS
                //=====================================================//
                //...
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.WARNING_PROCESS, product);

                //=====================================================//
                // 2. SUCCESS_PROCESS
                //=====================================================//
                await _productService.AddProductAsync(product);             

                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.SUCCESS_PROCESS, product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                //=====================================================//
                // 3. EXCEPTION_PROCESS
                //=====================================================//
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.EXCEPTION_PROCESS, product, ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            //=====================================================//
            // 1. START_PROCESS
            //=====================================================//
            processName = nameof(UpdateProduct);
            TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.START_PROCESS, product);
            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................
                if (id != product.Id)
                {
                    //=====================================================//
                    // 2. WARNING_PROCESS
                    //=====================================================//
                    processName = nameof(UpdateProduct) + $" Product ID mismatch: expected {id}, received {product.Id}";
                    TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.WARNING_PROCESS, product);
                    return BadRequest("Product ID mismatch");
                }

                //=====================================================//
                // 3. SUCCESS_PROCESS
                //=====================================================//
                await _productService.UpdateProductAsync(product);               
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName + $" Updating product with ID: {id}", LoggingConstants.START_PROCESS, product);
                return NoContent();
            }
            catch (Exception ex)
            {
                //=====================================================//
                // 4. EXCEPTION_PROCESS  
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.EXCEPTION_PROCESS, product, ex);
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //=====================================================//
            // 1. START_PROCESS
            //=====================================================//
            processName = nameof(UpdateProduct); 
            TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.START_PROCESS, id);

            try
            {
                // ..............................
                //*** Validation Logic
                // ..............................
                if (id <= 0)
                {

                    processName = nameof(DeleteProduct) + $" Invalid product ID: {id}";
                    //=====================================================//
                    // 2. WARNING_PROCESS
                    //=====================================================//
                    TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.WARNING_PROCESS);
                } 
              
                //=====================================================//
                // 3. SUCCESS_PROCESS
                //=====================================================//
                await _productService.DeleteProductAsync(id);               
   
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.SUCCESS_PROCESS, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                //=====================================================//
                // 4. EXCEPTION_PROCESS
                //=====================================================//
                TelemetryHelper.LogProcess(_logger, _telemetryClient, processName, LoggingConstants.EXCEPTION_PROCESS, ex);

                return StatusCode(500, "Internal server error");
            }
        }


    }
}
