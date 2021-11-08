#nullable enable

namespace Snittlistan.Web.Models
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;
    using Raven.Imports.Newtonsoft.Json;
    using Snittlistan.Queue.Messages;

    public class User
    {
        public const string AdminId = "Admin";

        private const string ConstantSalt = "CheFe2ra8en9SW";

        private Guid passwordSalt;
        private string activationKey;

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
        public string Id { get; set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user is activated.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the activation key.
        /// </summary>
        public string ActivationKey
        {
            get => activationKey ??= Guid.NewGuid().ToString();

            private set => activationKey = value;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string UniqueId { get; }

        private string HashedPassword { get; set; }

        private bool RequiresPasswordChange { get; set; }

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

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetPassword(string password)
        {
            HashedPassword = ComputeHashedPassword(password);
            RequiresPasswordChange = false;
        }

        public void SetPassword(string password, string suppliedActivationKey)
        {
            if (RequiresPasswordChange == false)
            {
                throw new InvalidOperationException("Password change not allowed");
            }

            if (ActivationKey != suppliedActivationKey)
            {
                throw new InvalidOperationException("Unknown activation key");
            }

            HashedPassword = ComputeHashedPassword(password);
            RequiresPasswordChange = false;
        }

        public bool ValidatePassword(string somePassword)
        {
            return RequiresPasswordChange == false
                && HashedPassword == ComputeHashedPassword(somePassword);
        }

        /// <summary>
        /// Initializes a new user. Must be done for new users.
        /// </summary>
        public void Initialize(Action<ITask> publish)
        {
            ActivationKey = Guid.NewGuid().ToString();
            publish.Invoke(new NewUserCreatedTask(Email, ActivationKey, Id));
        }

        /// <summary>
        /// Activates a user. This allows them to log on.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Activates a user and sends an invite email. This allows them to log on.
        /// </summary>
        public void ActivateWithEmail(Action<ITask> publish, UrlHelper urlHelper, string urlScheme)
        {
            IsActive = true;
            ActivationKey = Guid.NewGuid().ToString();
            RequiresPasswordChange = true;
            string activationUri = urlHelper.Action("SetPassword", "User", new
            {
                id = Id,
                activationKey = ActivationKey
            },
            urlScheme);
            Debug.Assert(activationUri != null, "activationUri != null");
            publish.Invoke(new UserInvitedTask(activationUri, Email));
        }

        /// <summary>
        /// Deactivates a user. This prevents them from logging on.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
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
}
