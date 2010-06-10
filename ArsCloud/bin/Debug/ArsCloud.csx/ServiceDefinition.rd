<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ArsCloud" generation="1" functional="0" release="0" Id="bd2e40c0-cb82-442b-8cdb-2889abd1bb55" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ArsCloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ArsCloudWeb:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ArsCloud/ArsCloudGroup/LB:ArsCloudWeb:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ArsCloudWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWebInstances" />
          </maps>
        </aCS>
        <aCS name="ArsCloudWeb:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWeb:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="ArsCloudWeb:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWeb:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="ArsCloudWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="ArsCloudWorker:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWorker:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="ArsCloudWorker:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ArsCloud/ArsCloudGroup/MapArsCloudWorker:DataConnectionString" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ArsCloudWeb:HttpIn">
          <toPorts>
            <inPortMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWeb/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapArsCloudWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWebInstances" />
          </setting>
        </map>
        <map name="MapArsCloudWeb:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWeb/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapArsCloudWeb:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWeb/DataConnectionString" />
          </setting>
        </map>
        <map name="MapArsCloudWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWorkerInstances" />
          </setting>
        </map>
        <map name="MapArsCloudWorker:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWorker/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapArsCloudWorker:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWorker/DataConnectionString" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ArsCloudWeb" generation="1" functional="0" release="0" software="c:\users\drpizza\documents\visual studio 2010\Projects\ArsCloud\ArsCloud\bin\Debug\ArsCloud.csx\roles\ArsCloudWeb" entryPoint="base\x64\WaWebHost.exe" parameters="" memIndex="1792" hostingEnvironment="frontendfulltrust" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" />
            </componentports>
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ArsCloudWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ArsCloudWeb&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ArsCloudWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWebInstances" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ArsCloudWorker" generation="1" functional="0" release="0" software="c:\users\drpizza\documents\visual studio 2010\Projects\ArsCloud\ArsCloud\bin\Debug\ArsCloud.csx\roles\ArsCloudWorker" entryPoint="base\x64\WaWorkerHost.exe" parameters="" memIndex="1792" hostingEnvironment="consolerolefulltrust" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ArsCloudWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ArsCloudWeb&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ArsCloudWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWorkerInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="ArsCloudWebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ArsCloudWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e6a09b50-a186-40d5-9b34-bb733bd85a59" ref="Microsoft.RedDog.Contract\ServiceContract\ArsCloudContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="9468ca68-1a31-4855-b796-abf03b717099" ref="Microsoft.RedDog.Contract\Interface\ArsCloudWeb:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ArsCloud/ArsCloudGroup/ArsCloudWeb:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>