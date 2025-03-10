using System;
using System.Runtime.CompilerServices;
using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CustomerDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerceApp.Services;

public class CustomerServices
{
    private readonly ApplicationDbContext _context;

    public CustomerServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CustomerResponseDTO>> RegisterCustomerAsync(CustomerRegistrationDTO customerDto)
    {
        try
        {
              // Check if email already exists
              if(await _context.Customers.AnyAsync(x => x.Email.ToLower() == customerDto.Email.ToLower()))
              {
                  return new ApiResponse<CustomerResponseDTO>(400, "Email already exists");
              }
            //Manual mapping from DTO to Model
            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
                DateOfBirth = customerDto.DateOfBirth,
                IsActive = true,
                //Hash the password using BCrypt
                Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password)
            };
            //Add customer to the database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            //Prepare response data
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };
            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);

        }
        catch (Exception ex)
        {
            // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            // Find customer by email with AsNoTracking for performance since we don't need to track changes
            var customer = await _context.Customers.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if(customer == null)
            {
                return new ApiResponse<LoginResponseDTO>(401,"Invalid email or password");
            }
            //Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password,customer.Password);
            
            if(!isPasswordValid)
            {
                return new ApiResponse<LoginResponseDTO>(401,"Invalid email or password");
            }

            //Prepare response Data
            var loginResponse = new LoginResponseDTO
            {
                Message = "Login Successful",
                CustomerId = customer.Id,
                CustomerName = $"{customer.FirstName} {customer.LastName}"
            };

            return new ApiResponse<LoginResponseDTO>(200, loginResponse);
        }
        catch (Exception ex)
        {
            //Log the exception
                return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }

    }

    public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync (int id)
    {
        try
        {
            var customer = await _context.Customers.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);

            if(customer == null)
            {
                return new ApiResponse<CustomerResponseDTO>(404, "Customer not found");
            }

            //Map to CustomerResponseDTO
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };
            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            // Log the exception
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpadateDTO customerDto)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(customerDto.CustomerId);

            if(customer == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            // Check if email is being updated to an existing one
                if (customer.Email != customerDto.Email && await _context.Customers.AnyAsync(c => c.Email == customerDto.Email))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Email is already in use.");
                }
            
            //Update customer properties manually
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.DateOfBirth = customerDto.DateOfBirth;

            await _context.SaveChangesAsync();

            //Prepare confirmation message
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {customerDto.CustomerId} updated successfully"
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }


    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync (int id)
    {
        try
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer ==null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
            }

            //Soft Delete
            customer.IsActive = false;
            await _context.SaveChangesAsync();

            //Prepare confirmation message
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Customer with Id {id} deleted successfully."
            };

            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
    }   

    // Changes the password for an existing customer.
    public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(changePasswordDto.CustomerId);
                if (customer == null || !customer.IsActive)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive.");
                }
                // Verify current password
                bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
                if (!isCurrentPasswordValid)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(401, "Current password is incorrect.");
                }
                // Hash the new password
                customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                await _context.SaveChangesAsync();
                // Prepare confirmation message
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = "Password changed successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }

        }
}
