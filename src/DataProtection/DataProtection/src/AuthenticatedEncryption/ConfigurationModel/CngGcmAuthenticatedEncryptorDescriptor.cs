// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Versioning;
using System.Xml.Linq;

namespace Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel
{
    /// <summary>
    /// A descriptor which can create an authenticated encryption system based upon the
    /// configuration provided by an <see cref="CngGcmAuthenticatedEncryptorConfiguration"/> object.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class CngGcmAuthenticatedEncryptorDescriptor : IAuthenticatedEncryptorDescriptor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CngGcmAuthenticatedEncryptorDescriptor"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="CngCbcAuthenticatedEncryptorConfiguration"/>.</param>
        /// <param name="masterKey">The master key.</param>
        public CngGcmAuthenticatedEncryptorDescriptor(CngGcmAuthenticatedEncryptorConfiguration configuration, ISecret masterKey)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (masterKey == null)
            {
                throw new ArgumentNullException(nameof(masterKey));
            }

            Configuration = configuration;
            MasterKey = masterKey;
        }

        internal ISecret MasterKey { get; }

        internal CngGcmAuthenticatedEncryptorConfiguration Configuration { get; }

        /// <inheritdoc />
        public XmlSerializedDescriptorInfo ExportToXml()
        {
            // <descriptor>
            //   <!-- Windows CNG-GCM -->
            //   <encryption algorithm="..." keyLength="..." [provider="..."] />
            //   <masterKey>...</masterKey>
            // </descriptor>

            var encryptionElement = new XElement("encryption",
                new XAttribute("algorithm", Configuration.EncryptionAlgorithm),
                new XAttribute("keyLength", Configuration.EncryptionAlgorithmKeySize));
            if (Configuration.EncryptionAlgorithmProvider != null)
            {
                encryptionElement.SetAttributeValue("provider", Configuration.EncryptionAlgorithmProvider);
            }

            var rootElement = new XElement("descriptor",
                new XComment(" Algorithms provided by Windows CNG, using Galois/Counter Mode encryption and validation "),
                encryptionElement,
                MasterKey.ToMasterKeyElement());

            return new XmlSerializedDescriptorInfo(rootElement, typeof(CngGcmAuthenticatedEncryptorDescriptorDeserializer));
        }
    }
}
