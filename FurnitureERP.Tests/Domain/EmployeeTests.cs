using FurnitureERP.Domain.Aggregates.Employees;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Tests.Domain;

public class EmployeeTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateEmployee()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        employee.FirstName.Should().Be("John");
        employee.LastName.Should().Be("Doe");
        employee.Position.Should().Be("Carpenter");
        employee.HourlyRate.Should().Be(250m);
        employee.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyFirstName_ShouldThrowException()
    {
        var action = () => new Employee("", "Doe", "Carpenter", 250m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void Create_WithEmptyLastName_ShouldThrowException()
    {
        var action = () => new Employee("John", "", "Carpenter", 250m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void Create_WithZeroHourlyRate_ShouldThrowException()
    {
        var action = () => new Employee("John", "Doe", "Carpenter", 0m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void Create_WithNegativeHourlyRate_ShouldThrowException()
    {
        var action = () => new Employee("John", "Doe", "Carpenter", -100m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void FullName_ShouldReturnCorrectFormat()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        employee.FullName.Should().Be("Doe John");
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateEmployee()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        employee.Update("Jane", "Smith", "Painter", 300m);

        employee.FirstName.Should().Be("Jane");
        employee.LastName.Should().Be("Smith");
        employee.Position.Should().Be("Painter");
        employee.HourlyRate.Should().Be(300m);
    }

    [Fact]
    public void UpdateHourlyRate_WithValidRate_ShouldUpdate()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        employee.UpdateHourlyRate(350m);

        employee.HourlyRate.Should().Be(350m);
    }

    [Fact]
    public void UpdateHourlyRate_WithZero_ShouldThrowException()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        var action = () => employee.UpdateHourlyRate(0m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void CalculateLaborCost_WithValidHours_ShouldReturnCorrectCost()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        var cost = employee.CalculateLaborCost(8m);

        cost.Should().Be(2000m);
    }

    [Fact]
    public void CalculateLaborCost_WithZeroHours_ShouldReturnZero()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        var cost = employee.CalculateLaborCost(0m);

        cost.Should().Be(0m);
    }

    [Fact]
    public void CalculateLaborCost_WithNegativeHours_ShouldThrowException()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        var action = () => employee.CalculateLaborCost(-5m);

        action.Should().Throw<InvalidEmployeeDataException>();
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldDeactivate()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);

        employee.Deactivate();

        employee.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_WhenInactive_ShouldActivate()
    {
        var employee = new Employee("John", "Doe", "Carpenter", 250m);
        employee.Deactivate();

        employee.Activate();

        employee.IsActive.Should().BeTrue();
    }
}
