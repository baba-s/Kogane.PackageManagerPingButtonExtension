using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kogane.Internal
{
    [InitializeOnLoad]
    internal sealed class PackageManagerPingButtonExtension
        : VisualElement,
          IPackageManagerExtension
    {
        private bool        m_isInitialized;
        private PackageInfo m_selectedPackageInfo;

        static PackageManagerPingButtonExtension()
        {
            var extension = new PackageManagerPingButtonExtension();
            PackageManagerExtensions.RegisterExtension( extension );
        }

        VisualElement IPackageManagerExtension.CreateExtensionUI()
        {
            m_isInitialized = false;
            return this;
        }

        void IPackageManagerExtension.OnPackageSelectionChange( PackageInfo packageInfo )
        {
            Initialize();

            m_selectedPackageInfo = packageInfo;
        }

        private void Initialize()
        {
            if ( m_isInitialized ) return;

            VisualElement root = this;

            while ( root is { parent: { } } )
            {
                root = root.parent;
            }

            var pingButton = new Button
            (
                () =>
                {
                    var assetPath = m_selectedPackageInfo.assetPath;
                    var asset     = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( assetPath );

                    EditorGUIUtility.PingObject( asset );
                }
            )
            {
                text = "Ping",
            };

            var removeButton = root.FindElement( x => x.name == "PackageRemoveCustomButton" );
            removeButton.parent.Insert( 0, pingButton );

            m_isInitialized = true;
        }

        void IPackageManagerExtension.OnPackageAddedOrUpdated( PackageInfo packageInfo )
        {
        }

        void IPackageManagerExtension.OnPackageRemoved( PackageInfo packageInfo )
        {
        }
    }
}