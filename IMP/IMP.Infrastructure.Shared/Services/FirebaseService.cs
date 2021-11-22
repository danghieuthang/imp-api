using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using IMP.Application.Interfaces;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System.IO;
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
        private async Task<FirebaseAuthLink> LoginAsync()
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseSettings.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseSettings.Email, _firebaseSettings.Password);
            return a;
        }
        /// <summary>
        /// The file stream to firebase storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="subfoders">subfolers</param>
        /// <returns>The file url</returns>
        public async Task<string> UploadFile(Stream stream, string fileName = "untitle.png", params string[] subfoders)
        {
            // Authentication
            var a = await LoginAsync();
            // Construct
            var firebaseStorage = new FirebaseStorage(
                _firebaseSettings.StorageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

            FirebaseStorageReference firebaseStorageRef;
            if (subfoders.Length > 0)
            {
                firebaseStorageRef = firebaseStorage.Child(subfoders[0]);
            }
            else
            {
                firebaseStorageRef = firebaseStorage.Child("");
            }

            for (int i = 1; i < subfoders.Length; i++)
            {
                firebaseStorageRef = firebaseStorageRef.Child(subfoders[i]);
            }

            firebaseStorageRef = firebaseStorageRef.Child(fileName);

            var task = firebaseStorageRef.PutAsync(stream);
            string url = await task;
            return url;
        }

        public async Task PushTotification(string data, params string[] subfoders)
        {
            var a = await LoginAsync();
            var firebaseClient = new FirebaseClient(baseUrl: _firebaseSettings.RealtimeDatabase,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                });

            ChildQuery childQuery;
            if (subfoders.Length > 0)
            {
                childQuery = firebaseClient.Child(subfoders[0]);
            }
            else
            {
                childQuery = firebaseClient.Child("");
            }

            for (int i = 1; i < subfoders.Length; i++)
            {
                childQuery = childQuery.Child(subfoders[i]);
            }

            await childQuery.PutAsync(data);
        }
    }
}
