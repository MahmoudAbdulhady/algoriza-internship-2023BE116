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

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost("/Settings/CreateCoupon")]
        public async Task<IActionResult> CreateFiftyDiscountCoupon([FromForm]CreateCouponDTO model)
        {
            await _couponService.CreateCouponAsync(model);
            return Ok($"CouponCode: {model.CouponCode} \n CouponName: {model.CouponName} \n CouponStatus: {model.IsActive}");
        }

        [HttpPost("/Settings/DeactivateCoupon")]
        public async Task<IActionResult> DeactivateCoupon(int couponId)
        {
            await _couponService.DeactivateCouponAsync(couponId);
            return Ok($"The Coupon With ID: {couponId} is Deactivated Sucessfully!");
        }

        [HttpPost("/Settings/DeleteCoupon")]
        public async Task<IActionResult> DeleteCouponById(int couponId)
        {
            await _couponService.DeleteCouponAsync(couponId);
            return Ok($"The Coupon With ID: {couponId} is deleted sucessfully");
        }

        [HttpPost("/Settings/UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon([FromForm]CouponUpdateDTO model)
        {
            await _couponService.UpdateCouponAsnyc(model);
            return Ok($"The Coupon With ID: {model} is updated sucessfully");
        }
    }
}
