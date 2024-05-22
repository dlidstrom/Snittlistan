using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Raven.Imports.Newtonsoft.Json;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Models;
public class User
{
    public const string AdminId = "Admin";

    private const string ConstantSalt = "CheFe2ra8en9SW";

    private Guid passwordSalt;

    public User(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HashedPassword = ComputeHashedPassword(password);
        UniqueId = Guid.NewGuid().ToString();
    }

    [JsonConstructor]
    private User(
        string firstName,
        string lastName,
        string email,
        string hashedPassword,
        Guid passwordSalt,
        bool requiresPasswordChange,
        string uniqueId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HashedPassword = hashedPassword;
        this.passwordSalt = passwordSalt;
        RequiresPasswordChange = requiresPasswordChange;
        UniqueId = uniqueId ?? Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the user is activated.
    /// </summary>
    public bool IsActive { get; private set; }

    public string FirstName { get; }

    public string LastName { get; }

    public string UniqueId { get; }

    private string HashedPassword { get; set; }

    private Guid PasswordSalt
    {
        get
        {
            if (passwordSalt == default)
            {
                passwordSalt = Guid.NewGuid();
            }

            return passwordSalt;
        }
    }

    public void SetEmail(string email, string password)
    {
        Email = email;
        HashedPassword = ComputeHashedPassword(password);
        RequiresPasswordChange = false;
    }

    public bool ValidatePassword(string somePassword)
    {
        return RequiresPasswordChange == false
            && HashedPassword == ComputeHashedPassword(somePassword);
    }

    private string ComputeHashedPassword(string password)
    {
        string hashedPassword;
        using SHA256 sha = SHA256.Create();
        byte[] computedHash = sha.ComputeHash(
            PasswordSalt.ToByteArray().Concat(
                Encoding.Unicode.GetBytes(password + PasswordSalt + ConstantSalt))
                .ToArray());

        hashedPassword = Convert.ToBase64String(computedHash);
        return hashedPassword;
    }
}
