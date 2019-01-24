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

namespace Nehta.Xsp.Sample
{
  public class SampleCertificateVerifier : ICertificateVerifier
  {
    public void Verify(X509Certificate2 certificate)
    {
      // Sample implementation that just prints a message. Here the 
      // certificate would be verified for validity. This would include
      // CRL checking, expiration and trust chain checking.
          
      Console.WriteLine("Verifying certificate: " + certificate.Subject);
    }
  }
}
