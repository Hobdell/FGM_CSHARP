using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace hscic.gov.uk.fhir.interop.fgm
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the individual resources used within the message bundle

            Parameters resourceParameters = CreateResources.CreateQueryResource();
            Organization resourceOrganization = CreateResources.CreateOrganizationResource();
            Practitioner resourcePractitioner = CreateResources.CreatePractitionerResource();
            MessageHeader resourceMessageHeader = CreateResources.CreateMessageHeaderResource();

            // Serialize to XML, this is the same as the XML examples within the DMS
            string x1 = FhirSerializer.SerializeResourceToXml(resourceParameters);
            string x2 = FhirSerializer.SerializeResourceToXml(resourceOrganization);
            string x3 = FhirSerializer.SerializeResourceToXml(resourcePractitioner);
            string x4 = FhirSerializer.SerializeResourceToXml(resourceMessageHeader);

            // Create the Bundle message as per the DMS
            //
            // Create the MessageHeader resource

            Bundle msg = new Bundle();

            // Add a logical id for this resource
            msg.Id = "13daadee-26e1-4d6a-9e6a-7f4af9b58877";

            // Add the profile for this resource (from the FGM DMS) 
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-Bundle-QueryParameters" };
            msg.Meta = metadata;

            // Set the message type
            msg.Type = Bundle.BundleType.Message;

            // Add the 1st bundle entry, the MessageHeader resource created above
            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceMessageHeader
            });

            // Add the 2nd bundle entry, the Parameters resource created above
            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceParameters
            });

            // Add the 3rd bundle entry, the Practitioner resource created above
            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourcePractitioner
            });

            // Add the 4th bundle entry, the Organization resource created above
            msg.Entry.Add(new Bundle.BundleEntryComponent()
            {
                Resource = resourceOrganization
            });

            // Serialize to XML, this is the same as the XML example within the DMS
            string x5 = FhirSerializer.SerializeResourceToXml(msg);
        }
    }
}
