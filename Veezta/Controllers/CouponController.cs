using Application.Contracts;
using Application.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Veezta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<CouponController> _logger;

        public CouponController(ICouponService couponService, ILogger<CouponController> logger)
        {
            _couponService = couponService;
            _logger = logger;
        }

        [HttpPost("/Settings/CreateCoupon")]
        public async Task<IActionResult> CreateFiftyDiscountCoupon([FromForm]CreateCouponDTO model)
        {
            _logger.LogInformation("Action Started");
            await _couponService.CreateCouponAsync(model);
            _logger.LogInformation("Action Ended");
            return Ok($"CouponCode: {model.CouponCode} \n CouponName: {model.CouponName} \n CouponStatus: {model.IsActive}");
        }

        [HttpPost("/Settings/DeactivateCoupon")]
        public async Task<IActionResult> DeactivateCoupon(int couponId)
        {
            _logger.LogInformation("Action Started");
            await _couponService.DeactivateCouponAsync(couponId);
            _logger.LogInformation("Action Ended");
            return Ok($"The Coupon With ID: {couponId} is Deactivated Sucessfully!");
            
        }

        [HttpPost("/Settings/DeleteCoupon")]
        public async Task<IActionResult> DeleteCouponById(int couponId)
        {
            _logger.LogInformation("Action Started");
            await _couponService.DeleteCouponAsync(couponId);
            _logger.LogInformation("Action Ended");
            return Ok($"The Coupon With ID: {couponId} is deleted sucessfully");
        }

        [HttpPost("/Settings/UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon([FromForm]CouponUpdateDTO model)
        {
            _logger.LogInformation("Action Started");
            await _couponService.UpdateCouponAsync(model);
            _logger.LogInformation("Action Ended");
            return Ok($"The Coupon With ID: {model} is updated sucessfully");
        }
    }
}
