namespace ChemSolution_re_API.Services.Pay
{
    public interface IPayService
    {
        PayButtonModel GetPayButton(PayOptions options);
    }
}
