namespace Updater
{
    /// <summary>
    /// Interface for communication between the main application and the update plugin.
    /// Allows requesting permissions and installing APK from third-party sources.
    /// </summary>
    /// <summary xml:lang="ru">
    /// »нтерфейс дл€ взаимодействи€ основного приложени€ с плагином обновлени€.
    /// ѕозвол€ет запрашивать разрешени€ и устанавливать APK из сторонних источников.
    /// </summary>
    public interface IUpdaterBridge
    {
        /// <summary>
        /// Requests permission to install apps from unknown sources
        /// (Android: Settings.ACTION_MANAGE_UNKNOWN_APP_SOURCES).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the permission is already granted or the user granted it successfully;
        /// <c>false</c> if the user denied it or an error occurred.
        /// </returns>
        /// <summary xml:lang="ru">
        /// «апрашивает у пользовател€ разрешение на установку приложений из неизвестных источников
        /// (Android: Settings.ACTION_MANAGE_UNKNOWN_APP_SOURCES).
        /// </summary>
        /// <returns>
        /// <c>true</c> Ч если разрешение уже есть или пользователь его успешно предоставил;
        /// <c>false</c> Ч если пользователь отказал или произошла ошибка.
        /// </returns>
        public bool RequestInstallUnknownSourcesPermission();

        /// <summary>
        /// Starts the installation of an APK file from the specified path.
        /// </summary>
        /// <param name="filePath">Full path to the APK file on the device
        /// (e.g. /storage/emulated/0/Download/app.apk).</param>
        /// <remarks>
        /// The implementation must correctly handle different Android versions
        /// (PackageInstaller for API 24+, legacy Intent.ACTION_INSTALL_PACKAGE for older versions).
        /// </remarks>
        /// <summary xml:lang="ru">
        /// «апускает установку APK-файла по указанному пути.
        /// </summary>
        /// <param name="filePath">ѕолный путь к APK-файлу на устройстве
        /// (например, /storage/emulated/0/Download/app.apk).</param>
        /// <remarks>
        /// –еализаци€ должна корректно работать на разных верси€х Android
        /// (PackageInstaller дл€ API 24+, старый Intent.ACTION_INSTALL_PACKAGE дл€ более ранних версий).
        /// </remarks>
        public void InstallApk(string filePath);
    }
}