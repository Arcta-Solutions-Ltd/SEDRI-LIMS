using arc.common.Models;
using arc.common.Utils;
using arc.domain.Security.User;
using arc.identity;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Security;

/// <summary>
/// Handles user registration, updating, and password management processes.
/// </summary>
public class RegisterHandler : IRegisterHandler
{
    private string _validationMessage = "";
    private readonly IGenericRepository _genericRepository;
    private readonly IJsonMapper _jsonMapper;
    private readonly IPasswordUtils _passwordUtils;
    private readonly IJsonUtils _jsonUtils;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterHandler"/> class.
    /// </summary>
    /// <param name="genericRepository">The generic repository for generic data operations.</param>
    /// <param name="jsonMapper">The JSON mapper used for transforming messages.</param>
    /// <param name="passwordUtils">The password utility for hashing and validating passwords.</param>
    /// <param name="jsonUtils">The JSON utility for extracting values from messages.</param>
    /// <param name="userRepository">The repository for user-specific operations.</param>
    public RegisterHandler(IGenericRepository genericRepository, IJsonMapper jsonMapper, IPasswordUtils passwordUtils, IJsonUtils jsonUtils, IUserRepository userRepository)
    {
        _genericRepository = genericRepository;
        _jsonMapper = jsonMapper;
        _passwordUtils = passwordUtils;
        _jsonUtils = jsonUtils;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the registration of a new user by comparing passwords, transforming the message,
    /// hashing the password, and saving the user data.
    /// </summary>
    /// <param name="message">A JSON string containing the user's registration details.</param>
    public void Handle(string message)
    {
        // Compare passwords to ensure that Password and ConfirmPassword match.
        if (_jsonUtils.GetSingleFieldValue(message, "Password") != _jsonUtils.GetSingleFieldValue(message, "ConfirmPassword"))
        {
            _validationMessage = "Passwords must be the same";
        }
        if (!string.IsNullOrEmpty(_validationMessage)) { return; }

        // Hash the password and transform the message to ensure the 'Enabled' flag is set.
        var userMapping = @"[{Type: 'New', Target: 'Enabled', Value: 'Yes'}]";
        var transformedMessage = _jsonMapper.Transform(message, userMapping, true);
        var newUser = JsonConvert.DeserializeObject<User>(transformedMessage);
        var newPassword = _passwordUtils.GetPassword(newUser);
        var newPasswordMapping = @"[{Type: 'Mapping', Source: 'Password', Target: 'Password', Value: '" + newPassword + "'}," +
                                    "{Type: 'Remove', Source: 'ConfirmPassword'}]";
        message = _jsonMapper.Transform(message, newPasswordMapping, true);

        // Save the transformed user data.
        _genericRepository.AddConfiguration("users");
        _genericRepository.AddAsync(message);
    }

    /// <summary>
    /// Updates an existing user's details. Validates necessary fields, hashes the password,
    /// and calls the appropriate method to either add or edit the user.
    /// </summary>
    /// <param name="message">A JSON string containing the user's updated details.</param>
    /// <param name="edit">
    /// If set to <c>true</c>, the method updates an existing user; otherwise, it adds a new user.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateUser(string message, bool edit)
    {
        var newUser = JsonConvert.DeserializeObject<User>(message);

        if (string.IsNullOrEmpty(newUser.Username)) { _validationMessage = "@UseUse@"; }
        if (string.IsNullOrEmpty(newUser.Roles)) { _validationMessage = "@UseRol@"; }

        if (!edit)
        {
            if (string.IsNullOrEmpty(newUser.Password)) { _validationMessage = "@UsePas@"; }
            else
            {
                if (newUser.Password.Length < 10) { _validationMessage = "@UsePasB@"; }
            }
        }

        if (string.IsNullOrEmpty(newUser.LaboratoryId) && string.IsNullOrEmpty(newUser.OrganisationId))
        {
            _validationMessage = "@UseEit@";
        }

        if (string.IsNullOrEmpty(_validationMessage))
        {
            if (edit)
            {
                await _userRepository.EditUserAsync(newUser);
            }
            else
            {
                newUser.Password = _passwordUtils.GetPassword(newUser);
                await _userRepository.AddUserAsync(newUser);
            }
        }
    }

    /// <summary>
    /// Changes a user's password by validating the new password and updating it in the repository.
    /// </summary>
    /// <param name="message">A JSON string containing the user's new password details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ChangePassword(string message)
    {
        var newUser = JsonConvert.DeserializeObject<User>(message);

        if (string.IsNullOrEmpty(newUser.Username)) { _validationMessage = "@UseUse@"; }
        if (string.IsNullOrEmpty(newUser.Password)) { _validationMessage = "@UsePas@"; }
        else
        {
            if (newUser.Password.Length < 10) { _validationMessage = "@UsePasB@"; }
        }

        if (string.IsNullOrEmpty(_validationMessage))
        {
            newUser.Password = _passwordUtils.GetPassword(newUser);
            await _userRepository.ChangePasswordAsync(newUser);
        }
    }

    /// <summary>
    /// Changes the current user's password by validating the new password, associating it with the user's
    /// token information, and updating it in the repository.
    /// </summary>
    /// <param name="message">A JSON string containing the new password.</param>
    /// <param name="token">A token containing the current user's identification details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ChangeMyPassword(string message, TokenInfoModel token)
    {
        var newUser = JsonConvert.DeserializeObject<User>(message);
        newUser.Id = int.Parse(token.Id);

        if (string.IsNullOrEmpty(newUser.Password)) { _validationMessage = "@UsePas@"; }
        else
        {
            if (newUser.Password.Length < 10) { _validationMessage = "@UsePasB@"; }
        }

        if (string.IsNullOrEmpty(_validationMessage))
        {
            newUser.Password = _passwordUtils.GetPassword(newUser);
            await _userRepository.ChangePasswordAsync(newUser);
        }
    }

    /// <summary>
    /// Retrieves the last validation message generated during the registration process.
    /// </summary>
    /// <returns>
    /// A string containing the validation message. If no validation error occurred, this returns an empty string.
    /// </returns>
    public string GetValidationMessage()
    {
        return _validationMessage;
    }
}
