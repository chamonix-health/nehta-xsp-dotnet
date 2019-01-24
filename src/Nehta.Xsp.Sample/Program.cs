/*
 * Copyright 2010 NEHTA
 *
 * Licensed under the NEHTA Open Source (Apache) License; you may not use this
 * file except in compliance with the License. A copy of the License is in the
 * 'license.txt' file, which should be provided with this work.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Nehta.Xsp.Impl.Utils;

namespace Nehta.Xsp.Sample
{
  /// <summary>
  /// Sample showing the use of the signed and encrypted containers to create
  /// a signed/encrypted payload.
  /// </summary>
  class Program
  { 
    /// <summary>
    /// Certificate thumbprint of the signing certificate.
    /// </summary>
    private const string XspTest1Thumbprint =
      "e3 1f 6e 5e b6 d2 93 7f a9 52 7e 88 64 db 92 61 61 73 cf f0";

    /// <summary>
    /// Certificate thumbprint of the encryption certificate.
    /// </summary>
    private const string XspTest2Thumbprint =
      "e5 ad a5 be 2f 1a ce be bb 7e 25 c3 8e 4f e4 bd b2 af fb 27";

    /// <summary>
    /// Test data to sign and encrypt.
    /// </summary>
    private const string DataPath = "Data\\sensitive_data.xml";
    
    /// <summary>
    /// Xsp version to used for the sample.
    /// </summary>
    private const XspVersion SampleXspVersion = XspVersion.V_2010;
    
  
    static void Main(string[] args)
    {
      // Load the payload to sign and encrypt
      XmlDocument payloadDoc = XmlUtils.LoadXmlDocument(DataPath);

      // Retrieve the certificate for signing
      X509Certificate2 signCert = X509StoreUtils.GetCertificate(
        XspTest1Thumbprint, X509FindType.FindByThumbprint);

      // Retrieve the certificate for encrypting
      X509Certificate2 encryptCert = X509StoreUtils.GetCertificate(
        XspTest2Thumbprint, X509FindType.FindByThumbprint);

      // Create the signed payload container
      Console.WriteLine("Signing the payload");
      ISignedContainerProfileService signedContainerService =
        XspFactory.Instance.GetSignedContainerProfileService(
        SampleXspVersion);
      XmlDocument signedContainerDoc =
        signedContainerService.Create(payloadDoc, signCert);

      // Create the encrypted payload container
      Console.WriteLine("Encrypting the payload");
      IEncryptedContainerProfileService encryptedContainerService =
        XspFactory.Instance.GetEncryptedContainerProfileService(
        SampleXspVersion);
      XmlDocument encryptedContainerDoc =
        encryptedContainerService.Create(signedContainerDoc, encryptCert);
      Console.WriteLine(encryptedContainerDoc.InnerXml);

      // Decrypt the payload
      Console.WriteLine("Decrypting the payload");
      XmlDocument decryptedSignedContainerDoc =
        encryptedContainerService.GetData(encryptedContainerDoc, encryptCert);
      Console.WriteLine(decryptedSignedContainerDoc.InnerXml);

      // Validate the signature
      // Note the check method throws an exception when signature validation fails
      Console.WriteLine("Validating the signature");
      signedContainerService.Check(decryptedSignedContainerDoc, 
        new SampleCertificateVerifier());

      // Get the data from the signed container
      XmlDocument decryptedPayloadDoc =
        signedContainerService.GetData(decryptedSignedContainerDoc);
      Console.WriteLine(decryptedPayloadDoc.InnerXml);

      Console.WriteLine();
      Console.WriteLine("Press any key...");
      Console.ReadKey(true);
    }
  }
}
