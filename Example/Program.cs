using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Parameters resourceParameters = CreateResources.CreateQueryResource();
            Organization resourceOrganization = CreateResources.CreateOrganizationResource();
            Practitioner resourcePractitioner = CreateResources.CreatePractitionerResource();
            MessageHeader resourceMessageHeader = CreateResources.CreateMessageHeaderResource();

            string x1 = FhirSerializer.SerializeResourceToXml(resourceParameters);
            string x2 = FhirSerializer.SerializeResourceToXml(resourceOrganization);
            string x3 = FhirSerializer.SerializeResourceToXml(resourcePractitioner);
            string x4 = FhirSerializer.SerializeResourceToXml(resourceMessageHeader);


            Bundle msg = new Bundle();
            msg.Id = "13daadee-26e1-4d6a-9e6a-7f4af9b58877";
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-Bundle-QueryParameters" };
            msg.Meta = metadata;

            msg.Type = Bundle.BundleType.Message;

            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceMessageHeader
            });

            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceParameters
            });

            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourcePractitioner
            });

            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceOrganization
            });

            string x5 = FhirSerializer.SerializeResourceToXml(msg);
        }
    }
}
