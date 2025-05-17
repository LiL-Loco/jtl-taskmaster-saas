import { Metadata } from 'next'
import { JobList } from '@/components/jobs/job-list'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Overview } from "@/components/dashboard/overview"
import { RecentJobs } from "@/components/dashboard/recent-jobs"
import Link from 'next/link'

export const metadata: Metadata = {
  title: 'Dashboard - JTL TaskMaster',
  description: 'Manage your JTL-Wawi automation tasks',
}

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Dashboard</h1>
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Aktive Jobs</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">12</div>
          </CardContent>
        </Card>
        {/* Weitere Statistik-Karten */}
      </div>
      <div className="grid gap-6 md:grid-cols-2">
        <Overview />
        <RecentJobs />
      </div>
    </div>
  )
}