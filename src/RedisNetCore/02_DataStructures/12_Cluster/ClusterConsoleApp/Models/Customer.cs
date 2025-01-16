using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ClusterConsoleApp.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
}

public class Address
{
    public string City { get; set; }
}

public class CustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string Nazwisko { get; set; }
    public DateTime Birthday { get; set; }
    public Address HomeAddress { get; set; }
}

[Mapper]
public partial class CustomerMapper
{
    [MapProperty(nameof(Customer.LastName), nameof(CustomerDto.Nazwisko))]
    public partial CustomerDto ToCustomerDto(Customer customer);

}