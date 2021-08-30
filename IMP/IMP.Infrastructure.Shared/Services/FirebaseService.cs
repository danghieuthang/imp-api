using Firebase.Auth;
using Firebase.Storage;
using IMP.Application.Interfaces;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IMP.Infrastructure.Shared.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseSettings _firebaseSettings;
        public FirebaseService(IOptions<FirebaseSettings> options)
        {
            _firebaseSettings = options.Value;
        }

        /// <summary>
        /// The file stream to firebase storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="subfoder">The name of forder in firebase storage</param>
        /// <returns>The file url</returns>
        public async Task<string> UploadFile(Stream stream, string subfoder, string fileName="untitle.png")
        {
            // Authentication
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseSettings.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseSettings.Email, _firebaseSettings.Password);
            // Construct
            var task = new FirebaseStorage(
                _firebaseSettings.StorageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(subfoder)
                .Child(fileName)
                .PutAsync(stream);
            string url = await task;
            return url;
        }
    }
}
