// src/app/layout.tsx
import QueryProvider from '@/providers/query-provider'
import { Toaster } from 'sonner'
import './globals.css'

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body>
        <QueryProvider>
          {children}
          <Toaster 
            position="top-right"
            expand={false}
            richColors
          />
        </QueryProvider>
      </body>
    </html>
  )
}
