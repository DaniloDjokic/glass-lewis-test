namespace Application.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(int companyId)
        : base($"The provided company ID {companyId} does not exist.")
    {
    }

    public CompanyNotFoundException(int companyId, Exception innerException)
        : base($"The provided company ID {companyId} does not exist", innerException)
    {
    }
}
