using AutoMapper;
using LeapPlannerApi.Entities.Login;
using LeapPlannerApi.Model;
using LeapPlannerApi.Model.Login;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeapPlannerApi.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;

        public LoginController(ILoginService loginService, IMapper mapper, IConfiguration configuration)
        {
            _loginService = loginService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Route("api/RegisterUser")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<RegisterUserReply> RegisterUser(RegisterUser registerUser)
        {
            RegisterUserReply reply = new RegisterUserReply();
            try
            {
                if (RegisterUserValidator(registerUser))
                {
                    var response = await _loginService.AddUser(_mapper.Map<RegisterUserDto>(registerUser));
                    if (response)
                    {
                        reply = ResponseService.UpdateSucess<RegisterUserReply>();
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<RegisterUserReply>(ErrorMessage.InvalidProcess.ToString());
                    }                               
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<RegisterUserReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch(Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<RegisterUserReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/Login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<LoginReply> Login(Login login)
        {
            LoginReply reply = new LoginReply();
            try
            {
                if (LoginValidator(login))
                {
                    var response = await _loginService.LoginUser(_mapper.Map<LoginDto>(login));
                    if (!String.IsNullOrEmpty(response))
                    {
                        reply = ResponseService.UpdateSucess<LoginReply>();
                        reply.Token = new JwtToken()
                        {
                            Access_Token = response,
                            ExpireIn = _configuration.GetValue<string>("Jwt:LifeSpan"),
                            Scope = "All"
                        };
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<LoginReply>(ErrorMessage.InvalidProcess.ToString());
                    }                
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<LoginReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch(Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<LoginReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/IsEmailExist")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IsEmailExistReply> IsEmailExist(IsEmailExist isEmailExist)
        {
            IsEmailExistReply reply = new IsEmailExistReply();
            try
            {
                if (EmailValidator(isEmailExist.Email))
                {
                    var response = await _loginService.GetUserDetails(isEmailExist.Email);
                    if (response != null)
                    {
                        reply = ResponseService.UpdateSucess<IsEmailExistReply>();
                        reply.IsUserExist = true;
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<IsEmailExistReply>(ErrorMessage.InvalidProcess.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<IsEmailExistReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch (Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<IsEmailExistReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/GenerateOtp")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<GenerateOtpReply> GenerateOtp(GenerateOtp generateOtp)
        {
            GenerateOtpReply reply = new GenerateOtpReply();
            try
            {
                if (EmailValidator(generateOtp.Email))
                {
                    var response = await _loginService.GenerateOtp(generateOtp.Email);
                    if (response)
                    {
                        reply = ResponseService.UpdateSucess<GenerateOtpReply>();
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<GenerateOtpReply>(ErrorMessage.InvalidProcess.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<GenerateOtpReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch (Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<GenerateOtpReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/ValidateOtp")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ValidateOtpReply> ValidateOtp(ValidateOtp validateOtp)
        {
            ValidateOtpReply reply = new ValidateOtpReply();
            try
            {
                if (EmailValidator(validateOtp.Email) && validateOtp.Otp.Length == 6)
                {
                    var response = await _loginService.ValidateOtp(validateOtp.Email, validateOtp.Otp);
                    if (response)
                    {
                        reply = ResponseService.UpdateSucess<ValidateOtpReply>();
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<ValidateOtpReply>(ErrorMessage.InvalidProcess.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<ValidateOtpReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch (Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<ValidateOtpReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/UpdatePassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<UpdatePasswordReply> UpdatePassword(Login updatePassword)
        {
            UpdatePasswordReply reply = new UpdatePasswordReply();
            try
            {
                if (LoginValidator(updatePassword))
                {
                    var response = await _loginService.UpdateUserPassword(_mapper.Map<LoginDto>(updatePassword));
                    if (response)
                    {
                        reply = ResponseService.UpdateSucess<UpdatePasswordReply>();
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<UpdatePasswordReply>(ErrorMessage.InvalidProcess.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<UpdatePasswordReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch (Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<UpdatePasswordReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }



        #region Validator
        private bool RegisterUserValidator(RegisterUser registerUser)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(registerUser.Email.Trim()) && !string.IsNullOrEmpty(registerUser.UserName.Trim()) && !string.IsNullOrEmpty(registerUser.Password.Trim()))
            {
                bool isEmail = Regex.IsMatch(registerUser.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                bool isPassword = Regex.IsMatch(registerUser.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
                isValid = isEmail && isPassword;
            }
            return isValid;
        }

        private bool LoginValidator(Login login)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(login.Email.Trim()) && !string.IsNullOrEmpty(login.Password.Trim()))
            {
                bool isEmail = Regex.IsMatch(login.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                bool isPassword = Regex.IsMatch(login.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
                isValid = isEmail && isPassword;
            }
            return isValid;
        }

        private bool EmailValidator(string email)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(email.Trim()))
            {
                isValid = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);;
            }
            return isValid;
        }
        #endregion Validator
    }
}
