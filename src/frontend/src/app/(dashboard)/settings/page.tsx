// src/frontend/src/app/(dashboard)/settings/page.tsx
'use client'

import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { ProfileSettings } from "@/components/settings/profile-settings"
import { EmailSettings } from "@/components/settings/email-settings"
import { FtpSettings } from "@/components/settings/ftp-settings"

export default function SettingsPage() {
  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Einstellungen</h1>
      <Tabs defaultValue="profile">
        <TabsList>
          <TabsTrigger value="profile">Profil</TabsTrigger>
          <TabsTrigger value="email">E-Mail</TabsTrigger>
          <TabsTrigger value="ftp">FTP</TabsTrigger>
        </TabsList>
        <TabsContent value="profile">
          <ProfileSettings />
        </TabsContent>
        <TabsContent value="email">
          <EmailSettings />
        </TabsContent>
        <TabsContent value="ftp">
          <FtpSettings />
        </TabsContent>
      </Tabs>
    </div>
  )
}
