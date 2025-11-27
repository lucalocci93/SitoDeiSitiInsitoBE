using AutoMapper;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;
using SitoDeiSiti.Models.ConfigSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SitoDeiSiti.Utils.Encryption;
using Microsoft.Extensions.Caching.Hybrid;
using SitoDeiSiti.Backend.Interfaces;

namespace SitoDeiSiti.Backend.Services
{
    public class UserManager : BaseManager, IUser
    {
        private readonly IDalUtente dalUtente;
        private readonly IOptions<Token> TokenSettings;

        private enum CacheKey
        {
            GetCinture,
            GetOrganizzazioni
        }

        public UserManager(IOptions<Token> tokenSettings, SitoDeiSitiInsitoContext context, IMapper mapper, HybridCache hybridCache) 
            : base(mapper, hybridCache)
        {
            dalUtente = new DalUtenti(context);
            TokenSettings = tokenSettings;
        }

        public async Task<Response<User>> CreateUser(User _user)
        {
            User user = new User();

            //Encryption.Encrypt<User>(_user);

            try
            {
                if(await dalUtente.GetUtenteByMail(_user.Email).ConfigureAwait(false))
                {
                    return new Response<User>(false, new Error(ErrorCode.EmailInUso, "Email gia utilizzata da un altro utente"));
                }

                _user.IsAdmin = false;

                Utente utente = Mapper.Map<Utente>(_user);
                UtenteInfo utenteInfo = Mapper.Map<UtenteInfo>(_user);
                UtentePrivacy utentePrivacy = Mapper.Map<UtentePrivacy>(_user);
                UtenteAtleta utenteAtleta = Mapper.Map<UtenteAtleta>(_user);

                bool result = await dalUtente.CreateUtente(utente, utenteInfo, utentePrivacy, utenteAtleta).ConfigureAwait(false);

                if (result) 
                {
                    return new Response<User>(true, user);
                }
                else
                {
                    throw new Exception("Errore inserimento utente");
                }
            }
            catch(Exception ex)
            {
                return new Response<User>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<JWT>> GenerateToken(string username, string password)
        {
            JWT jwt = new JWT();

            //string key = new Guid().ToString();
            try
            {
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(TokenSettings.Value.SecretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                Utente? utente = new Utente();
                User user = new();

                utente = await dalUtente.CheckUtenteUserAndPassword(username, password).ConfigureAwait(false);

                //Encryption.Decrypt<Utente>(user);

                if (utente != null)
                {
                    user = Mapper.Map<Utente, User>(utente);
                    Guid? org = utente.UtenteAtleta?.Organizzazione;

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Nome),
                        new Claim(ClaimTypes.Surname, user.Cognome),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("CodFiscale", user.CodFiscale),
                        new Claim(ClaimTypes.Role, user.IsAdmin.HasValue && user.IsAdmin.Value ? "Admin" : "User"),
                        new Claim("SportRole", user.IsMaestro.HasValue && user.IsMaestro.Value ? "Maestro" : "Atleta"),
                        new Claim("Org", org.HasValue ? org.Value.ToString() : "nessuna"),
                        new Claim("sub", user.RowGuid.ToString())
                        // Other custom data (claims)
                    };

                    var token = new JwtSecurityToken(
                        issuer: TokenSettings.Value.Issuer,
                        audience: TokenSettings.Value.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(TokenSettings.Value.expireMinutes),
                        signingCredentials: credentials
                    );

                    var tokenHandler = new JwtSecurityTokenHandler();
                    jwt.Token = tokenHandler.WriteToken(token);


                    return new Response<JWT>(true, jwt);
                }
                else
                {
                    return new Response<JWT>(false, new Error("Utente o Password errati"));
                }
            }
            catch (Exception ex)
            {
                return new Response<JWT>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<User>>> GetAllUser()
        {
            var users = new List<User>();

            try
            {
                users = Mapper.Map<List<Utente?>, List<User>>(await dalUtente.GetUtenti().ConfigureAwait(false));

                //Encryption.Decrypt<List<User>>(users);

                return new Response<List<User>>(true, users);
            }
            catch (Exception ex)
            {
                return new Response<List<User>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Belts>>> GetCinture()
        {
            List<Belts> belts = new();

            try
            {
                belts = await HybridCache.GetOrCreateAsync<List<Belts>>(nameof(CacheKey.GetCinture), async result => Mapper.Map<List<Belts>>(await dalUtente.GetCinture().ConfigureAwait(false)));

                if (belts != null && belts.Any())
                {
                    return new Response<List<Belts>>(true, belts);
                }
                else
                {
                    return new Response<List<Belts>>(false, new Error("Nessuna categoria trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Belts>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<Organization>>> GetOrganizzazioni()
        {
            List<Organization> organization = new();

            try
            {
                organization = await HybridCache.GetOrCreateAsync<List<Organization>>(nameof(CacheKey.GetOrganizzazioni), async result => Mapper.Map<List<Organization>>(await dalUtente.GetOrganizzazioni().ConfigureAwait(false)));

                if (organization != null && organization.Any())
                {
                    return new Response<List<Organization>>(true, organization);
                }
                else
                {
                    return new Response<List<Organization>>(false, new Error("Nessuna categoria trovata"));
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Organization>>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<User>> GetUser(Guid RowGuid)
        {
            var user = new User();

            try
            {
                Utente? utente = await dalUtente.GetUtente(RowGuid).ConfigureAwait(false);
                if(utente != null)
                {
                    user = Mapper.Map<Utente, User>(utente);

                    user.Abbonamenti = Mapper.Map<ICollection<Abbonamento>, List<Subscription>>(utente.Abbonamento);
                    //user = Mapper.Map<UtenteInfo, User>(utente.UtenteInfo);
                    return new Response<User>(true, user);
                }
                else
                {
                    return new Response<User>(false, user);
                }
            }
            catch (Exception ex)
            {
                return new Response<User>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<User>> UpdateUser(UserDbOperationEnum operation, User _user)
        {
            int updatedRow = 0;
            try
            {
                Utente utente = Mapper.Map<User, Utente>(_user);
                UtenteInfo utenteInfo = Mapper.Map<User, UtenteInfo>(_user);
                UtentePrivacy utentePrivacy = Mapper.Map<User,UtentePrivacy>(_user);
                UtenteAtleta utenteAtleta = Mapper.Map<User, UtenteAtleta>(_user);

                if (utente != null && utenteInfo != null && utentePrivacy != null)
                {

                    updatedRow = await dalUtente.UpdateUser(operation, utente, utenteInfo, utentePrivacy, utenteAtleta).ConfigureAwait(false);

                    if (updatedRow != 0)
                    {
                        return new Response<User>(true, _user);
                    }
                    else
                    {
                        throw new Exception("Errore Aggiornamento");
                    }
                }
                else
                {
                    throw new Exception("Errore Aggiornamento");
                }
            }
            catch (Exception ex)
            {
                return new Response<User>(false, new Error(ex.Message));
            }
        }

        public async Task<Response<List<User>>> GetAtletiOrganizzazione(Guid Org)
        {
            var users = new List<User>();

            try
            {
                users = Mapper.Map<List<Utente>, List<User>>(await dalUtente.GetUtentiOrganizzazioni(Org).ConfigureAwait(false));

                //Encryption.Decrypt<List<User>>(users);

                return new Response<List<User>>(true, users);
            }
            catch (Exception ex)
            {
                return new Response<List<User>>(false, new Error(ex.Message));
            }
        }
    }
}
